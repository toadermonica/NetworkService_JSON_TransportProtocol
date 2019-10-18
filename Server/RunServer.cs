using System;
using System.Threading;

namespace Server
{
    class RunServer
    {
        static void Main(string[] args)
        {
            string ipAddress = "127.0.0.1";
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                Server myserver = new Server(ipAddress, 13000);
            });
            t.Start();

            Console.WriteLine("Server Started...!");
        }
    }
}
