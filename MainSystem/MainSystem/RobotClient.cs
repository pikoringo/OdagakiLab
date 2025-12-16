using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace MainSystem
{
    public class RobotClient
    {
        private Form1 form;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Thread commThread;
        private Thread writeThread;

        public bool flagHideAliveMessages = true; 
        private bool flagStopRequest = false;
        private bool flagThreadReadRunning = false;
        private bool flagConnected = false;
        private bool flagThreadWriteRunning = false;

        public double[] posJointsCurrent = new double[7];
        public double[] posCartesian = new double[6];
        public double currentX, currentY, currentZ, currentB;
        public bool moving = false;
        public bool targetReached = false;
        public bool goforward = false;

        private readonly double[] jogValues = new double[9];

        public event Action<double[], double[]> PositionUpdated;
        // (joint[], cartesian[]) for UI or logging

        public RobotClient(string ip, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            stream = tcpClient.GetStream();
            stream.ReadTimeout = 100;
        }

        public void Connect()
        {
            if (!flagConnected)
            {
                flagStopRequest = false;
                flagConnected = true;

                // Start write thread (ALIVEJOG / queued commands)
                if (writeThread == null)
                {
                    writeThread = new Thread(WriteLoop)
                    {
                        Priority = ThreadPriority.Normal,
                        Name = "WriteLoop",
                        IsBackground = true,
                        CurrentCulture = new System.Globalization.CultureInfo("en-US")
                    };
                    writeThread.Start();
                }

                // Start read thread (status messages)
                if (commThread == null)
                {
                    commThread = new Thread(ReadLoop)
                    {
                        Priority = ThreadPriority.Normal,
                        Name = "ReadLoop",
                        IsBackground = true,
                        CurrentCulture = new System.Globalization.CultureInfo("en-US")
                    };
                    commThread.Start();
                }
            }
            else
            {
                string msg = "Cannot reconnect - already connected!";
                Console.WriteLine(msg);
            }
        }
        void WriteLoop()
        {
            int sleepTime = 100;
            flagThreadWriteRunning = true;
            try
            {
                while (!flagStopRequest)
                {
                    // this string should be 256 char long max, otherwise it may not be read completly
                    string msg = "ALIVEJOG "
                        + jogValues[0].ToString("0.0") + " " + jogValues[1].ToString("0.0") + " " + jogValues[2].ToString("0.0") + " "
                        + jogValues[3].ToString("0.0") + " " + jogValues[4].ToString("0.0") + " " + jogValues[5].ToString("0.0") + " "
                        + jogValues[6].ToString("0.0") + " " + jogValues[7].ToString("0.0") + " " + jogValues[8].ToString("0.0");
                    SendCommand(msg, flagHideAliveMessages);
                    Thread.Sleep(sleepTime);
                }
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("CRI write loop: {0}", ex.Message);
            }
            flagConnected = false;
            flagThreadWriteRunning = false;
            flagStopRequest = true;
            writeThread = null;
        }
        //public void StartReading()
        //{
        //    if (commThread == null)
        //    {
        //        commThread = new Thread(ReadLoop)
        //        {
        //            IsBackground = true,
        //            Name = "RobotReadLoop"
        //        };
        //        commThread.Start();
        //    }
        //}


        private void ReadLoop()
        {
            flagConnected = true;
            flagThreadReadRunning = true;
            byte[] buffer = new byte[4096];

            while (!flagStopRequest)
            {
                try
                {
                    if (!stream.DataAvailable)
                    {
                        Thread.Sleep(5);
                        continue;
                    }

                    string msg = ReadMessage(buffer);
                    if (msg != null)
                    {
                        ParseStatusString(msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("RobotClient ReadLoop Error: " + ex.Message);
                    Thread.Sleep(100);
                }
            }
            
            flagThreadReadRunning = false;
        }

        private string ReadMessage(byte[] buffer)
        {
            // Wait for CRISTART header
            bool gotStart = false;
            while (stream.DataAvailable)
            {
                for (int i = 0; i < 7; i++) buffer[i] = buffer[i + 1];
                buffer[7] = (byte)stream.ReadByte();

                if (Encoding.ASCII.GetString(buffer, 0, 8) == "CRISTART")
                {
                    gotStart = true;
                    break;
                }
            }

            if (!gotStart) return null;

            // Read until CRIEND
            int cnt = 8;
            while (true)
            {
                buffer[cnt] = (byte)stream.ReadByte();
                cnt++;
                if (Encoding.ASCII.GetString(buffer, cnt - 6, 6) == "CRIEND")
                    break;
            }

            return Encoding.ASCII.GetString(buffer, 0, cnt);
        }

        private void ParseStatusString(string msg)
        {
            string[] parts = msg.Split(' ');
            if ((parts.Length < 46) || (parts[0] != "CRISTART")) return;

            CultureInfo culInf = CultureInfo.InvariantCulture;

            // joints
            for (int i = 23; i <= 29; i++)
                posJointsCurrent[i - 23] = double.Parse(parts[i], culInf);

            // cartesian
            for (int i = 40; i <= 45; i++)
                posCartesian[i - 40] = double.Parse(parts[i], culInf);

            currentX = posCartesian[0];
            currentY = posCartesian[1];
            currentZ = posCartesian[2];
            currentB = posCartesian[4];

            //moving = false; // update with your own motion detection logic
            CheckPos();
            

            PositionUpdated?.Invoke(posJointsCurrent, posCartesian);
        }

        public void SetJogValues(double[] jValues)
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    if (jValues[i] > 100.0) jValues[i] = 100.0;
                    if (jValues[i] < -100.0) jValues[i] = -100.0;
                    this.jogValues[i] = jValues[i];
                }

                // Always send joint motion type before sending jog
                //SendCommand("CMD MotionTypeJoint");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SetJogValues: " + ex.Message);
            }
        }

        public void SendCoordinatesToIrc(double x, double y, double z, double C)
        {
            // Constant ABC values
            const double A = 0;
            const double B = -180;
            //const double C = 36; //10-20 method
            //double C = TargetAngle;

            SendCommand("CMD Active true");

            string moveCommand = $"CMD Move Cart {x} 0 {z} {A} {-C} {B} 0 0 0 40";
            SendCommand(moveCommand);
            //moving = true; // Mark as moving when a move command is sent
            //
            //Console.WriteLine(moveCommand);
            //Console.WriteLine(moving);

        }
        public void SendCommand(string cmd)
        {
            SendCommand(cmd, false);
        }
        public void SendCommand(string cmd, bool hideAlive)
        {
            if (!tcpClient.Connected || stream == null) return;
            string message = $"CRISTART 0 {cmd} CRIEND\n";
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            
            if (!hideAlive)
            {
                Console.WriteLine(message);
                form?.Invoke((MethodInvoker)(() =>
                {
                    form.textBoxLogMessages.AppendText(message + Environment.NewLine);
                }));
            }
        }
        public void CheckPos()
        {
            //Console.WriteLine("Checking pos");
            double xdiff = Math.Abs(posCartesian[0] - Form1.lastX);
            //double ydiff = Math.Abs(posCartesian[1] - Form1.lastY);
            double zdiff = Math.Abs(posCartesian[2] - Form1.lastZ);

            if (xdiff < 1 &&
                    zdiff < 1)
            {
                //nasionXYZUpdated = false;
                //form.textBoxLogMessages.AppendText("Checking pos" + Environment.NewLine);
                targetReached = true;
                goforward = true;
                //Thread.Sleep(1000);
                //Console.WriteLine("checkpos");
            }

        }

        public void GoForward()
        {
            if (goforward)
            {
                SendCommand("CMD MotionTypeCartTool");
                //jogValues[2] = 10.0;
                jogValues[2] = Math.Max(jogValues[2] + 10.0, 40.0);
                SetJogValues(jogValues);
            }
        }
        public void Stop()
        {
            if (goforward)
            {
                jogValues[2] = 0;
                SetJogValues(jogValues);
            }
        }
        public void Disconnect()
        {
            flagStopRequest = true;
            stream?.Close();
            tcpClient?.Close();
        }
    }
}
