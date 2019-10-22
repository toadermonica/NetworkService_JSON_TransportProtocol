using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
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
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("client connected");

                    var thread = new Thread(ClientInstance);

                    thread.Start(client);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception thrown: {0}", e);
                server.Stop();
            }
           

            //server.Stop(); - this method is never hit due to the while loop - find a way to close server by input string in cmd - while msg ! exit then keep server alive - not mandatory
        }

        //Method needs to work like this due to it being called in the thread. The thread parses the TcpClient client object with .Start()
        public static void ClientInstance(object incomingObj)
        {
            Console.WriteLine("in client instance");
            try
            {
                TcpClient clientInstance = (TcpClient)incomingObj;
                ServerController serverController = new ServerController();
                serverController.HandleOpperation(clientInstance);
            }
            catch (Exception error)
            {
                var er = error;
                Console.WriteLine("Exception thrown: {0}", er);
            }

        }
    }

      
}
