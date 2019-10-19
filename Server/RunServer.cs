using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class RunServer
    {

        private const int portNumber = 5000;
        private const string ipAddress = "127.0.0.1";


        static void Main(string[] args)
        {
            //instantiate and start server
            var server = new TcpListener(IPAddress.Parse(ipAddress), portNumber);
            server.Start();
            Console.WriteLine("Server is up and running!");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                // Start a thread that calls a parameterized static method.
                Thread clientThread = new Thread(ClientInstance);
                clientThread.Start(client);
            }

            //server.Stop(); - this method is never hit due to the while loop - find a way to close server by input string in cmd - while msg ! exit then keep server alive - not mandatory
        }

        //Method needs to work like this due to it being called in the thread. The thread parses the TcpClient client object with .Start()
        public static void ClientInstance(object incomingObj)
        {
            try
            {
                TcpClient clientInstance = (TcpClient)incomingObj;

                var stream = clientInstance.GetStream();

                var buffer = new byte[clientInstance.ReceiveBufferSize];

                var rcnt = stream.Read(buffer, 0, buffer.Length);

                var msg = Encoding.UTF8.GetString(buffer, 0, rcnt);

                Console.WriteLine($"Message: {msg}");

                msg = msg.ToUpper();

                buffer = Encoding.UTF8.GetBytes(msg);

                stream.Write(buffer, 0, buffer.Length);

                stream.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine("Exception thrown: ", error);
            }
          
        }

    }

}
