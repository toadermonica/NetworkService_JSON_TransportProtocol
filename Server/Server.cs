using Server2;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Server
{
    class Server
    {
        TcpListener server = null;
        public Server(IPAddress localAddr, int port)
        {
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var client = server.AcceptTcpClient();

                    Console.WriteLine($"A client connected!");
                    Thread t = new Thread(new ParameterizedThreadStart(ClientInstance));
                    t.Start(client);

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public void ClientInstance(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            Response response = new Response();

            var buffer = new byte[client.ReceiveBufferSize];
            try
            {
                var request = client.ReadRequest();
                Console.WriteLine("Illegal json body? ", request);

                //pass request to mother validation function from validation class
                if (!Validation.isValidClientRequest(request, out string error, out string echoCaseBody))
                {
                    response.Status = error;
                    if (!string.IsNullOrEmpty(echoCaseBody))
                    {
                        response.Body = echoCaseBody;
                    }
                }

                

                Console.WriteLine("Incoming: {0}",request.ToString());
                Console.WriteLine("Response Status to be sent: {0}", response.Status);
                Console.WriteLine("Response Body: {0}", response.Body);

                var serializedObj = JsonSerializer.Serialize<Response>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                Console.WriteLine("serializedObj before write to stream: {0}", serializedObj);
                var byteReplyMsg = Encoding.UTF8.GetBytes(serializedObj);
                stream.Write(byteReplyMsg, 0, byteReplyMsg.Length);
                Console.WriteLine("{1}: Sent: {0}", response, Thread.CurrentThread.ManagedThreadId);
                
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e.ToString());
                Console.WriteLine("Exception"); 
                client.Close();
            }
        }
    }
}