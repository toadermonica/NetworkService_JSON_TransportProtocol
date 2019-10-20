using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Server
{
    public class ServerController
    {
        protected TcpClient clientInstance { get; set; }
        protected NetworkStream stream { get; set; }

        public ServerController(TcpClient clientInstance, NetworkStream stream)
        {
            this.clientInstance = clientInstance;
            this.stream = stream;
        }

        public string ClientRequest()
        {
            byte[] buffer = new byte[clientInstance.ReceiveBufferSize];
            int rcnt = stream.Read(buffer, 0, buffer.Length);
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, rcnt);
            return receivedMessage;
        }

        //Will take care of deserialization of json request 
        //and parse to object of type request and category depending on the case
        public bool isValidRequest(string receivedMessage, out string error)
        {
            Request request;
            Category requestBody;

            //check if request from client is of type JSON object
            try
            {
                 request = JsonConvert.DeserializeObject<Request>(receivedMessage);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                error = "invlaid JSON";
                return false;
            }

            //check if method provided is valid
            if(request.method == null)
            {
                error = "missing method";
                return false;
            }

            if (!request.method.Equals("create") && !request.method.Equals("read") && !request.method.Equals("update") && !request.method.Equals("delete") && !request.method.Equals("echo"))
            {
                error = "illegal method";
                return false;
            }
            //check if date provided is valid
            var requestDate = request.date;
            if(string.IsNullOrEmpty(requestDate.ToString()))
            {
                error = "missing date";
                return false;
            }
            var isLong = requestDate.GetType().Equals(typeof(long));
            if (!isLong)
            {
                error = "illegal date";
                return false;
            }
            
            //check if resource is valid
            var requestResource = request.path;
            if (string.IsNullOrEmpty(requestResource))
            {
                error = "missing resource";
                return false;
            }

            //check if body provided is valid
            if (string.IsNullOrEmpty(request.body))
            {
                error = "missing body";
                return false;
            }

            try
            {
                //try and deserialize the string to json object and if it is not json then catch error
                requestBody = JsonConvert.DeserializeObject<Category>(request.body);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                error = "illegal body";
                return false;
            }
            var bodyCid = requestBody.cid;
            var bodyName = requestBody.name;

            if (string.IsNullOrEmpty(bodyCid.ToString()))
            {
                error = "missing cid";
                return false;
            }
            if (string.IsNullOrEmpty(bodyName))
            {
                error = "missing bodyName";
                return false;
            }

            //if all is fine then return true and error will be empty
            error = string.Empty;
            return true;

        }


        public  void ServerResponse(string response)
        {
            //this is gonna be answer from server to client
            if (string.IsNullOrEmpty(response))
            {
                response = "This needs to do some status return instead!!!";
            }
            response = response.ToUpper(); //- need a server message/response to be set here
            // make the message into bytes again and encode
            byte[] msg = Encoding.ASCII.GetBytes(response);
            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent from server: {0}", response);
            // add/write to network stream for client
            stream.Write(msg, 0, msg.Length);
            //close stream with client
            stream.Close();
        }
        
    }
}

