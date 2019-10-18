using System;
using System.Threading;

namespace Client
{
    class RunClient
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            string ipAddress = "127.0.0.1";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                client.Connect(ipAddress, "Hello I'm Device 1...");
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                client.Connect(ipAddress, "Hello I'm Device 2...");
            }).Start();
            Console.ReadLine();
        }
    }
}
