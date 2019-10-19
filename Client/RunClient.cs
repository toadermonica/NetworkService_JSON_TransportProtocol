using NUnit.Framework;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class RunClient
    {
        static void Main(string[] args)
        {

            while (true)
            {

                //Constraint_ConnectionWithoutRequest_ShouldConnect();
                var client = new TcpClient();
                client.Connect(IPAddress.Loopback, 5000);

                var stream = client.GetStream();

                Console.WriteLine("Send message:");
                var msg = Console.ReadLine();
                var buffer = Encoding.UTF8.GetBytes(msg);
                stream.Write(buffer, 0, buffer.Length);

                if (msg == "exit") break;

                buffer = new byte[client.ReceiveBufferSize];
                var rcnt = stream.Read(buffer, 0, buffer.Length);
                msg = Encoding.UTF8.GetString(buffer, 0, rcnt);
                Console.WriteLine($"Response from server is: {msg}");
                stream.Close();
            }
        }


        /*public static void Constraint_ConnectionWithoutRequest_ShouldConnect()
        {
            TcpClient newClient = new TcpClient();
            newClient.Connect(IPAddress.Loopback, 5000);
            Assert.True(newClient.Connected);
        }*/

    }
}
