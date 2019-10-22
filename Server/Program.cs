using System;
using System.Net;


namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            new Server(IPAddress.Parse("127.0.0.1"), 5000);
            Console.WriteLine("Server Started");
        }

    }
}