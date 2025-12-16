using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;


namespace MainSystem
{
    public partial class Form1 : Form
    {
        private RealSenseCamera camera;
        private TextileSensor sensor;

        private bool locateC3 = false;
        //private C3Localization c3Localization;

        private RobotClient robotClient;
        private static NetworkStream stream;
        private static Thread tcpThread;
        public bool connected = false;
        private static bool running = true;
        private static bool moving = false;
        private static bool goforward = false;
        private static bool moveTo45 = false;
        public static bool load = false;
        // Variable to track the last time coordinates were sent
        private DateTime lastSendTime = DateTime.Now;
        private DateTime lastMovementTime = DateTime.Now;

        // Previous readings for XYZ coordinates
        public static double lastX = double.NaN;
        public static double lastY = double.NaN;
        public static double lastZ = double.NaN;
        private static double lastAngle = double.NaN;
        private static double targetX = double.NaN;
        private static double targetY = double.NaN;
        private static double targetZ = double.NaN;
        private static double targetAngle = double.NaN;
        private static double tiltTolerance = 0.15; // Allowable deviation from 0.5 (10%) for even load
        private double[] jogValues = new double[9]; // store current jog values for 9 joints

        private bool displayPaused = false;

        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Initialize camera
            camera = new RealSenseCamera();
            camera.FrameReady += Camera_FrameReady;
            camera.TargetComputed += Camera_C3TargetComputed;

            // Generate placeholder
            Bitmap placeholder = new Bitmap(pictureBoxCamera.Width, pictureBoxCamera.Height);
            using (var g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                string text = "Camera feed will appear here";
                using (var font = new Font("Arial", 10))
                {
                    var textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, Brushes.Black,
                        (pictureBoxCamera.Width - textSize.Width) / 2,
                        (pictureBoxCamera.Height - textSize.Height) / 2);
                }
            }
            pictureBoxCamera.Image = placeholder;
            pictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom;

            //Initialize sensors
            sensor = new TextileSensor();
            sensor.SensorDataUpdated += Sensor_SensorDataUpdated;
            sensor.Start("COM3", 38400);
            
        }
        private void Camera_FrameReady(Bitmap frame)
        {
            if (pictureBoxCamera.InvokeRequired)
            {
                pictureBoxCamera.Invoke(new Action(() =>
                {
                    pictureBoxCamera.Image?.Dispose();
                    pictureBoxCamera.Image = (Bitmap)frame.Clone();
                }));
            }
            else
            {
                pictureBoxCamera.Image?.Dispose();
                pictureBoxCamera.Image = (Bitmap)frame.Clone();
            }
        }

        private void buttonStartCamera_Click_1(object sender, EventArgs e)
        {
            if (camera != null)
            {
                string predictorPath = @"D:\MainSystem\shape_predictor_68_face_landmarks.dat";
                camera.Start(predictorPath);
            }
            else
            {
                MessageBox.Show("Camera not initialized!");
            }
        }
        private void buttonStopCamera_Click_1(object sender, EventArgs e)
        {
            locateC3 = false; // stop C3 overlay
            camera.Stop();

            // Restore placeholder
            Bitmap placeholder = new Bitmap(pictureBoxCamera.Width, pictureBoxCamera.Height);
            using (var g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                string text = "Camera feed will appear here";
                using (var font = new Font("Arial", 12))
                {
                    var textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, Brushes.Black,
                        (pictureBoxCamera.Width - textSize.Width) / 2,
                        (pictureBoxCamera.Height - textSize.Height) / 2);
                }
            }
            pictureBoxCamera.Image?.Dispose();
            pictureBoxCamera.Image = placeholder;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            camera.Dispose();
        }
        private void CRIConnect_Click(object sender, EventArgs e)
        {
            string IPAddress = textBoxIPAddress.Text;
            int Port = (int)textBoxPort.Value;

            tcpThread = new Thread(() => InitializeRobotClient(IPAddress, Port));
            tcpThread.Start();
            

            if (robotClient != null)
            {
                MessageBox.Show("Already connected!");
                return;
            }
        }
        private void InitializeRobotClient(string ip, int port)
        {
            try
            {
                robotClient = new RobotClient(ip, port);
                robotClient.PositionUpdated += RobotClient_PositionUpdated;
                robotClient.Connect();

                this.Invoke((MethodInvoker)delegate
                {
                    labelConnectionStatus.Text = $"Connected to {ip}";
                    connected = true;
                });

                // Send initial setup commands
                robotClient.SendCommand("MESSAGE CPRog Version V902-13-040");
                robotClient.SendCommand("MESSAGE Configuration: \"igus REBEL-5DOF\" Type: \"REBEL-5DOF-01\" Gripper: \"\"");
                robotClient.SendCommand("VelocitySetting: \"0\"");
                robotClient.SendCommand("CMD Active true");
                robotClient.SendCommand("CMD Enable");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void buttonC3Localization_Click(object sender, EventArgs e)
        {
            locateC3 = true; // Enable C3 detection and overlay
            camera.EnableC3(true);
            //Console.WriteLine("LocateC3 clicked");
            textBoxLogMessages.AppendText("Locating C3 point..." + Environment.NewLine);

        }

        private void buttonSendC3_Click(object sender, EventArgs e)
        {
            // Send one move command to the robot
            robotClient.SendCoordinatesToIrc(targetX, targetY, targetZ, targetAngle);
            lastX = targetX;
            lastY = targetY;
            lastZ = targetZ;
            lastAngle = targetAngle;
            jogValues[4] = 0.0;
            robotClient.SetJogValues(jogValues);
            textBoxLogMessages.AppendText("Target Location sent to Robot!" + Environment.NewLine);
        }
        private void Camera_C3TargetComputed(object sender, TargetEventArgs e)
        {
            if (!locateC3) return; // Only draw if C3 is active
            

            this.Invoke((Action)(() =>
            {
                if (!double.IsNaN(e.AdjustedX) && !double.IsNaN(e.AdjustedZ) && !moving)
                {
                    //Console.WriteLine(DateTime.Now - lastSendTime);

                    if ((DateTime.Now - lastSendTime).TotalSeconds >= 1)
                    {

                        // Store the most recent target coordinates
                        targetX = e.AdjustedX;
                        targetY = e.AdjustedY;
                        targetZ = e.AdjustedZ;
                        targetAngle = e.AdjustedAngle;

                        // Update labels for user feedback
                        labelTargetX.Text = $"X: {lastX:F2}";
                        labelTargetY.Text = $"Y: {lastY:F2}";
                        labelTargetZ.Text = $"Z: {lastZ:F2}";
                        labelTargetAngle.Text = $"θ: {lastAngle:F2}";
                    }
                }
                
            }));
        }

        private void Sensor_SensorDataUpdated(double adjCh0, double adjCh1, double normCh0, double normCh1, double totalLoad, double blCh0, double blCh1, bool baselineReady)
        {
            if (displayPaused) return; // <-- ignore updates when paused

            this.Invoke((Action)(() =>
            {
                //labelSensorData.Text = $"Ch0: {normCh0:F2}, Ch1: {normCh1:F2}";
                labelSensorStatus.Text = baselineReady ? "Calibrated!" : "Collecting Baseline...";
                labelBaseLineCh0.Text = $"BLCh0: {blCh0}";
                labelBaseLineCh1.Text = $"BLCh1: {blCh1}";
                labelNormCh0.Text = $"NrmCh0: {normCh0}";
                labelNormCh1.Text = $"NrmCh1: {normCh1}";
                labelAdjCh0.Text = $"AdjCh0: {adjCh0}";
                labelAdjCh1.Text = $"AdjCh1: {adjCh1}";

                if (totalLoad > 200)
                {
                    double expected = 0.5;
                    if (connected) { robotClient.Stop(); }

                    if (Math.Abs(normCh0 - expected) > tiltTolerance)
                    {
                        if (normCh0 > normCh1)
                        {
                            labelLoadStatus.Text = "Tilt towards Ch0";
                        }
                        else
                        {
                            labelLoadStatus.Text = "Tilt towards Ch1";
                        }
                    }
                    else
                    {
                        labelLoadStatus.Text = "Even Load";
                    }
                }
                else
                {
                    labelLoadStatus.Text = "No Load";
                    //if (connected) { robotClient.GoForward(); }
                }
            }));
        }

        private void RobotClient_PositionUpdated(double[] joints, double[] cartesian)
        {
            // This runs on a background thread, so invoke UI update
            this.Invoke((MethodInvoker)delegate
            {
                // Joints
                //string jointsString = "Joints SetPoint:\n" + string.Join("\n", robotClient.posJointsSetPoint.Take(7).Select(v => v.ToString("0.0")));
                string jointsCurrentString = "Joints Current:\n" + string.Join("\n", robotClient.posJointsCurrent.Take(7).Select(v => v.ToString("0.0")));
                //labelPositionJoints.Text = jointsString;
                labelPositionJointsCurrent.Text = jointsCurrentString;

                // Cartesian
                string posString = "Position Cart:\n" + string.Join("\n", robotClient.posCartesian.Take(6).Select(v => v.ToString("0.0")));
                labelPositionCart.Text = posString;
                double diff = Math.Abs(robotClient.posJointsCurrent[4] + 45);
                Console.WriteLine(diff);
                if (moveTo45 && diff < 0.5)
                {
                    //Console.WriteLine("hello!");
                    robotClient.SendCommand("CMD MotionTypeJoint");
                    jogValues[4] = 0.0;
                    robotClient.SetJogValues(jogValues);
                    moveTo45 = false;
                }
            });
        }
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (robotClient != null)
                {
                    robotClient.Disconnect();
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelConnectionStatus.Text = "Disconnected";
                        connected = false;
                        //labelStatus.Text = "Stopped";
                    });
                }

                if (tcpThread != null && tcpThread.IsAlive)
                {
                    tcpThread.Interrupt();
                    tcpThread = null;
                }

                MessageBox.Show("Connection closed.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disconnecting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonA5Minus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeJoint");
            jogValues[4] = Math.Max(jogValues[4] - 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void buttonA5Minus_MouseUp(object sender, MouseEventArgs e)
        {
            // Reset jog value to 0 when released
            jogValues[4] = 0.0;
            robotClient.SetJogValues(jogValues);
        }

        private void buttonA5Plus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeJoint");
            jogValues[4] = Math.Max(jogValues[4] + 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void buttonJogZMinus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeCartTool");
            jogValues[2] = Math.Max(jogValues[2] - 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void buttonJogZMinus_MouseUp(object sender, MouseEventArgs e)
        {
            jogValues[2] = 0.0;
            robotClient.SetJogValues(jogValues);
        }

        private void buttonJogZPlus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeCartTool");
            jogValues[2] = Math.Max(jogValues[2] + 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void buttonJogBMinus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeCartTool");
            jogValues[4] = Math.Max(jogValues[4] - 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void buttonJogBPlus_MouseDown(object sender, MouseEventArgs e)
        {
            robotClient.SendCommand("CMD MotionTypeCartTool");
            jogValues[4] = Math.Max(jogValues[4] + 10.0, -100.0);
            robotClient.SetJogValues(jogValues);
        }

        private void button45Angle_Click(object sender, EventArgs e)
        {
            moveTo45 = true;
            if (robotClient.posJointsCurrent[4] > -45)
            {
                robotClient.SendCommand("CMD MotionTypeJoint");
                jogValues[4] = Math.Max(jogValues[4] - 2.0, -100.0);
                robotClient.SetJogValues(jogValues);
            }
            else if (robotClient.posJointsCurrent[4] < -45)
            {
                robotClient.SendCommand("CMD MotionTypeJoint");
                jogValues[4] = Math.Max(jogValues[4] + 2.0, -100.0);
                robotClient.SetJogValues(jogValues);
            }
        }

        private void buttonPauseReading_Click(object sender, EventArgs e)
        {
            displayPaused = !displayPaused; // toggle
        }
    }
}
