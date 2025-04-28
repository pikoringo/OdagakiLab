using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    private static TcpClient? tcpClient;
    private static NetworkStream? stream;
    private static Thread? tcpThread;
    private static Thread? inputThread;
    private static bool running = true;
    private static bool isStopped = false;

    static void Main(string[] args)
    {

        // Initialize and start the TCP thread
        tcpThread = new Thread(new ThreadStart(TcpCommunication));
        tcpThread.Start();

        // Initialize and start the Arduino thread
        inputThread = new Thread(new ThreadStart(UserInput));
        inputThread.Start();


        Console.WriteLine("Type 'exit' to terminate the program.");
        Console.ReadKey();
        running = false;

        tcpThread.Join();
        inputThread.Join();
        Console.WriteLine("Program terminated.");
    }

    private static void TcpCommunication()
    {
        try
        {
            tcpClient = new TcpClient("127.0.0.1", 3920); //Input IP Address
            stream = tcpClient.GetStream();
            Console.WriteLine("TCP connection established.");

            // Send initialization commands
            SendCommand("MESSAGE CPRog Version V902-13-040");
            SendCommand("MESSAGE Configuration: \"igus REBEL-5DOF\" Type: \"REBEL-5DOF-01\" Gripper: \"\"");
            SendCommand("VelocitySetting: \"0\"");
            SendCommand("CMD Active true");
            SendCommand("CMD Enable");

            while (running)
            {
                // Construct the ALIVEJOG message based on input
                double jogValue = isStopped ? 0.0 : 20.0;
                SendCommand($"ALIVEJOG {jogValue:0.0} 0.0 0.0 0.0 0.0 0.0 0.0 0.0 0.0");
                
                Thread.Sleep(1000); // Send the ALIVEJOG message every second
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("TCP Error: " + ex.Message);
        }
        finally
        {
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
            Console.WriteLine("TCP connection closed.");
        }
    }

    private static void UserInput()
    {
        try
        {
            while (running)
            {
                Console.WriteLine("Press '1' to toggle stop/go.");
                string? input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "exit")
                {
                    running = false; // Exit condition for the program
                }
                else if (input == "1")
                {
                    // Toggle the state of the robot: stop or go
                    isStopped = !isStopped;
                    string state = isStopped ? "stopped" : "going";
                    Console.WriteLine($"Robot is now {state}.");
                }
                else
                {
                    // Handle any other input (if needed)
                    Console.WriteLine("Invalid input. Press '1' to toggle stop/go.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Input Error: " + ex.Message);
        }
    }

    private static void SendCommand(string command)
    {
        string fullCommand = $"CRISTART 0 {command} CRIEND\n";
        byte[] data = Encoding.ASCII.GetBytes(fullCommand);
        stream?.Write(data, 0, data.Length);
        //Console.WriteLine(fullCommand);
    }
}
