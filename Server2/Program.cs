using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server
{
    static class Method
    {
        public const string Read = "read";
        public const string Create = "create";
        public const string Update = "update";
        public const string Delete = "delete";

        public static readonly string[] methods = { Read, Create, Update, Delete };
    }
    public class Response
    {
        public string Status { get; set; }
        public string Body { get; set; }
    }
    public class Request
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return "Method: " + Method + ", Path: " + Path + ", Date: " + Date + ", Body: " + Body;
        }
    }

    public class Category
    {
        [JsonPropertyName("cid")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + Name;
        }
    }

    class Program
    {

        public static List<Category> categories = new List<Category>
    {
      new Category {Id = 1, Name = "Beverages"},
      new Category{Id = 2, Name = "Condiments"},
      new Category{Id = 3, Name = "Confections"}
    };

        public static readonly string[] methods = { };

        static void Main(string[] args)
        {
            new Server(IPAddress.Parse("127.0.0.1"), 5000);
            Console.WriteLine("Server Started");
        }

    }
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
                if(!Validation.hasBody(request, out string bodyError))
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

    public static class Validation
    {
        public static bool isValidMethodName(Request obj, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(obj.Method))
            {
                Console.WriteLine("Missing method");
                error = "missing method";
                return false;
            }
            if(!obj.Method.Equals("create") && !obj.Method.Equals("delete") && !obj.Method.Equals("read") && !obj.Method.Equals("update") && !obj.Method.Equals("echo"))
            {
                Console.WriteLine("Inside validator, illegal method");
                error = "illegal method";
                return false;
            }
            return true;
        }
        public static bool isValidPath(Request obj, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(obj.Path))
            {
                error = "missing resource";
                return false;
            }
            return true;
        }
        public static bool isValidDate(Request obj, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(obj.Date))
            {
                error = "missing date";
                return false;
            }
            int number = 0;
            if (!Int32.TryParse(obj.Date, out number))
            {
                Console.WriteLine("Number parsed: ", number);
                error = "illegal date";
                return false;
            }
            
            return true;
        }

        public static bool hasBody(Request obj, out string bodyError)
        {
            var method = obj.Method;
            var error = string.Empty;
            var body = obj.Body;
            Console.WriteLine("Body is in method: ", body);
            if(!method.Equals("delete") && !method.Equals("read") && isValidMethodName(obj, out error))
            {
                if (string.IsNullOrEmpty(body))
                {
                    bodyError = "missing body";
                    return false;
                }
            }
            if (method.Equals("update"))
            {
                Console.WriteLine("Body in illegal test: ", body);
            }
            Console.WriteLine("Body in illegal tes: ", obj.ToString());
            bodyError = string.Empty;
            return true;
        }

    }
    public static class Util
    {
        public static Response ReadResponse(this TcpClient client)
        {
            var strm = client.GetStream();
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream())
            {
                int bytesread = 0;
                do
                {
                    bytesread = strm.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);
                    var responseData2 = Encoding.UTF8.GetString(memStream.ToArray());
                } while (bytesread == 2048);

                var responseData = Encoding.UTF8.GetString(memStream.ToArray());
                return JsonSerializer.Deserialize<Response>(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }
        public static Request ReadRequest(this TcpClient client)
        {
            var strm = client.GetStream();
            //strm.ReadTimeout = 250;
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream())
            {
                int bytesread = 0;
                do
                {
                    bytesread = strm.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);
                    var responseData2 = Encoding.UTF8.GetString(memStream.ToArray());
                } while (bytesread == 2048);

                var requestData = Encoding.UTF8.GetString(memStream.ToArray());
                return JsonSerializer.Deserialize<Request>(requestData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }

        public static Boolean ArrayContains(string[] array, string stringToCheck)
        {
            if (stringToCheck == null)
            {
                return false;
            }
            foreach (string x in array)
            {
                if (stringToCheck.Contains(x))
                {
                    return true;
                }
            }
            return false;
        }
        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
        public static T FromJson<T>(this string element)
        {
            return JsonSerializer.Deserialize<T>(element, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}