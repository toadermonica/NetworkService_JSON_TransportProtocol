using System;
using System.Text;
using System.Net.Sockets;
using System.Text.Json;

namespace Server
{
    public class ServerController
    {
        public void HandleOpperation(TcpClient clientInstance)
        {
            NetworkStream stream = clientInstance.GetStream();
            if (stream.CanRead)
            {
                Console.WriteLine("handle operation");
                byte[] buffer = new byte[2048];
                string receivedMessage = string.Empty;
                var isDataOnStream = stream.DataAvailable;
                if (isDataOnStream)
                {
                    var rcnt = stream.Read(buffer, 0, buffer.Length);
                    receivedMessage = Encoding.UTF8.GetString(buffer, 0, rcnt);
                    Console.WriteLine(receivedMessage);
                    try
                    {
                        if (!string.IsNullOrEmpty(receivedMessage))
                        {
                            Console.WriteLine(receivedMessage);
                            var request = JsonSerializer.Deserialize<Request>(receivedMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            //call to see what method that deals with thre reponse;
                            //var responseToSend = GetMethodResponse(request, "invalid method");
                            var resp = isValidMethod(request);
                            Console.WriteLine("Echo? {0}", resp);
                            //string jsonServerResponse = JsonSerializer.Serialize(responseToSend, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            var msg = Encoding.UTF8.GetBytes(resp);
                            //var msg = Encoding.UTF8.GetBytes(jsonServerResponse);
                            Console.WriteLine(resp);
                            stream.Write(msg, 0, msg.Length);
                            stream.Close();
                        }else{
                            Console.WriteLine("No request given");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Faulty request, cannot deserialize");
                        // TODO: if no valid request then return invalid request. 
                    }
                }
                   
            }

            /*var method = request.Method;
            if (method.Equals("echo"))
            {
                var body = request.Body;
                var responseToSend = new Response();
                Status status = new Status();
                status.code = 1;
                status.statusBody = status.SetStatusBody(1);
                if (string.IsNullOrEmpty(body))
                {
                    responseToSend.status = status;
                    responseToSend.body = string.Empty;
                }
                else
                {
                    responseToSend.status = status;
                    responseToSend.body = body;
                }

                //write to stream
                string jsonServerResponse = JsonSerializer.Serialize(responseToSend, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var msg = Encoding.UTF8.GetBytes(jsonServerResponse);
                stream.Write(msg, 0, msg.Length);
                stream.Close();

            }*/

            
        }

        public string isValidMethod(Request request)
        {
            var method = request.Method;
           if(!method.Equals("create") && !method.Equals("read") && !method.Equals("update") && !method.Equals("delete") && !method.Equals("echo")){
                return string.Empty;
           }
            return string.Empty;
        }

        public string GetMethodResponse(Request request, string error)
        {
            var method = request.Method;
            switch (method)
            {
                case "echo":
                    return Echo(request.Body);
                default:
                    return error;
            }
        }

        public string Echo(string request)
        {

            return request;

            /* //write to stream
             string jsonServerResponse = JsonSerializer.Serialize(responseToSend, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
             var msg = Encoding.UTF8.GetBytes(jsonServerResponse);
             stream.Write(msg, 0, msg.Length);
             stream.Close();*/

        }

        public Response ErrorResponse(string error)
        {

            Status status = new Status();
            Response response = new Response();

            status.code = 4;
            status.statusBody = status.SetStatusBody(4);
            response.status = status;
            response.body = error;
            return response;
        }

    }
}

