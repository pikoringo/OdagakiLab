using System;
using System.IO.Ports;
using System.Timers;
using System.Threading;

namespace MainSystem
{
    public class TextileSensor : IDisposable
    {
        private SerialPort serialPort;
        private System.Timers.Timer requestTimer;
        private RobotClient robotClient;
        private Form1 form;

        private int baselineSamples = 50;
        private int baselineCount = 0;
        private double baselineCh0 = 0;
        private double baselineCh1 = 0;
        private bool baselineReady = false;

        private double normCh0 = 0;
        private double normCh1 = 0;
        private double tiltTolerance = 0.1;

        //Event to send updates to Form1
        public event Action<double, double, double, double, double, double, double, bool> SensorDataUpdated;
        public bool IsRunning { get; private set; } = false;

        public void Start(string portName = "COM3", int baudRate = 38400)
        {
            try
            {
                serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
                {
                    ReadTimeout = 500,
                    WriteTimeout = 500
                };

                serialPort.Open();
                Console.WriteLine("Serial port opened.");

                SendInitializationCommands();

                requestTimer = new System.Timers.Timer(100);
                requestTimer.Elapsed += SendRequest;
                requestTimer.AutoReset = true;
                requestTimer.Enabled = true;

                IsRunning = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sensor Init Error: " + ex.Message);
            }
        }

        private void SendInitializationCommands()
        {
            byte checksum1 = (byte)((0x44 + 0x0A + 0x01 + 0x00) & 0xFF);
            byte[] cmd1 = new byte[] { 0x44, 0x0A, 0x01, checksum1, 0x00 };
            serialPort.Write(cmd1, 0, cmd1.Length);
            Thread.Sleep(50);
            serialPort.ReadExisting();

            byte checksum2 = (byte)((0x44 + 0x02 + 0x03 + 0x00 + 0x00 + 0x01) & 0xFF);
            byte[] cmd2 = new byte[] { 0x44, 0x02, 0x03, checksum2, 0x00, 0x00, 0x01 };
            serialPort.Write(cmd2, 0, cmd2.Length);
            Thread.Sleep(50);
            serialPort.ReadExisting();
        }

        private void SendRequest(object sender, ElapsedEventArgs e)
        {
            if (!serialPort.IsOpen) return;

            try
            {
                byte checksum = (byte)((0x04 + 0x03 + 0x00) & 0xFF);
                byte[] request = new byte[] { 0x04, 0x03, 0x00, checksum, 0x00, 0x00, 0x01 };
                serialPort.Write(request, 0, request.Length);

                byte[] response = new byte[9];
                int bytesRead = serialPort.Read(response, 0, 9);

                if (bytesRead == 9)
                {
                    int ts0 = response[5] + (response[6] << 8);
                    int ts1 = response[7] + (response[8] << 8);

                    if (!baselineReady)
                    {
                        //form.textBoxLogMessages.AppendText("Sensors Calibrating" + Environment.NewLine);
                        baselineCh0 += ts0;
                        baselineCh1 += ts1;
                        baselineCount++;

                        if (baselineCount >= baselineSamples)
                        {
                            baselineCh0 /= baselineSamples;
                            baselineCh1 /= baselineSamples;
                            baselineReady = true;
                            Console.WriteLine($"Baseline ready: Ch0={baselineCh0:F2}, Ch1={baselineCh1:F2}");
                        }
                    }
                    else
                    {
                        double adjustedCh0 = Math.Max(0, ts0 - baselineCh0);
                        double adjustedCh1 = Math.Max(0, ts1 - baselineCh1);
                        double totalLoad = adjustedCh0 + adjustedCh1;

                        //if (totalLoad > 500)
                        //{
                            normCh0 = adjustedCh0 / totalLoad;
                            normCh1 = adjustedCh1 / totalLoad;
                        //}
                        //else
                        //{
                        //    normCh0 = 0;
                        //    normCh1 = 0;
                        //}

                        //Raise event for Form1
                        SensorDataUpdated?.Invoke(ts0, ts1, normCh0, normCh1, totalLoad, baselineCh0, baselineCh1, baselineReady);
                    }
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Sensor read timeout.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sensor Error: " + ex.Message);
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;
            requestTimer?.Stop();
            serialPort?.Close();
            IsRunning = false;
        }

        public void Dispose()
        {
            Stop();
            serialPort?.Dispose();
            requestTimer?.Dispose();
        }
    }
}

