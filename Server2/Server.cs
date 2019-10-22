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
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    t.Start(client);

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public void HandleDevice(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            Response response = new Response();

            var buffer = new byte[client.ReceiveBufferSize];
            try
            {
                var request = client.ReadRequest();
                Console.WriteLine("Illegal json body? ", request);
                if (!Validation.isValidMethodName(request, out string methodError))
                {
                    response.Status = methodError;
                }
                if(!Validation.isValidPath(request, out string pathError))
                {
                    response.Status = pathError;
                }
                if(!Validation.isValidDate(request, out string dateError))
                {
                    response.Status = dateError;
                }
                Console.WriteLine("Before entering:{0} ",request);
                if (!Validation.hasBody(request, out string bodyError))
                {
                    Console.WriteLine("HasBody is now : ", bodyError);
                    response.Status = bodyError;
                }

                Console.WriteLine("In hereee!!! main: ",request.ToString());
                Console.WriteLine("Response Status: ", response.Status);
                Console.WriteLine("Response Body: ", response.Status);

                var serializedObj = JsonSerializer.Serialize<Response>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var byteReplyMsg = Encoding.UTF8.GetBytes(serializedObj);
                stream.Write(byteReplyMsg, 0, byteReplyMsg.Length);
                Console.WriteLine("{1}: Sent: {0}", response, Thread.CurrentThread.ManagedThreadId);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }
    }
}