using Server2;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server
{
    class Server
    {
        static CategoryManager catman = new CategoryManager();
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
                Console.WriteLine("Incoming: {0}", request.ToString());

                response = Validation.isValidClientRequest(request, Server.catman);

                if (response.StatusNumber == 1) { response.Body = response.EchoBody; }
                else if (response.StatusNumber == 4 || response.StatusNumber == 5) {
                    response.Status = response.StatusNumber.ToString() + " " + response.Status; }
                else
                {
                    if (request.Method.Equals("create")) {
                        var test = JsonSerializer.Deserialize<Category>(request.Body);
                        Console.WriteLine(test);
                        Server.catman.add(test.Name);
                        response.StatusNumber = 2;
                        response.Status = "Created";
                        test.Id = Server.catman.GetCategoryId(test.Name);
                        response.Body = JsonSerializer.Serialize<Category>(test);

                    }
                    if (request.Method.Equals("delete")) {
                        string[] pathValues = Regex.Split(request.Path, @"\/");
                        Console.WriteLine("delok " + pathValues[3]);

                        foreach (Category cat in Server.catman.GetCategories())
                        { Console.WriteLine(cat); }

                        Server.catman.delete(Int32.Parse(pathValues[3]));

                        foreach (Category cat in Server.catman.GetCategories())
                        { Console.WriteLine(cat); }

                        response.StatusNumber = 1;
                        response.Status = "Ok";

                    }
                    if (request.Method.Equals("read")) {

                        string[] pathValues = Regex.Split(request.Path, @"\/");

                        try { 
                            Server.catman.GetCategory(Int32.Parse(pathValues[3]));
                            response.Body = Server.catman.GetCategory(Int32.Parse(pathValues[3])).ToJson();
                        }
                        catch
                        {
                            response.Body = Server.catman.GetCategories().ToJson();
                        }
                        response.StatusNumber = 1;
                        response.Status = "Ok";
                    }
                    if (request.Method.Equals("update")) {
                        string[] pathValues = Regex.Split(request.Path, @"\/");
                        var test = JsonSerializer.Deserialize<Category>(request.Body);
                        foreach (Category cat in Server.catman.GetCategories())
                        { Console.WriteLine(cat); }

                        Server.catman.update(Int32.Parse(pathValues[3]), test.Name);

                        response.StatusNumber = 3;
                        response.Status = "Updated";
                        foreach (Category cat in Server.catman.GetCategories())
                        { Console.WriteLine(cat); }
                    }
                    response.Status = response.StatusNumber.ToString() + " " + response.Status;
                }


                Console.WriteLine("Response Status to be sent: {0}", response.Status);
                Console.WriteLine("Response Body: {0}", response.Body);

                try
                {
                    var serializedObj = JsonSerializer.Serialize<Response>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    Console.WriteLine("serializedObj before write to stream: {0}", serializedObj);
                    var byteReplyMsg = Encoding.UTF8.GetBytes(serializedObj);
                    stream.Write(byteReplyMsg, 0, byteReplyMsg.Length);
                    Console.WriteLine("{1}: Sent: {0}", response, Thread.CurrentThread.ManagedThreadId);
                }
                catch (Exception) { Console.WriteLine("Connection lost!"); }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }
    }


}