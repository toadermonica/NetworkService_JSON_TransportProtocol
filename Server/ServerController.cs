using System;
using System.Text;
using System.Net.Sockets;
using Server.Helpers;
using Newtonsoft.Json;

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

        public string SetResponse(string receivedMessage)
        {
            Response serverResponse = new Response();
            Validation validator = new Validation(receivedMessage);
            string error;
            if (validator.isValidRequest(out error))
            {
                serverResponse = new Response(2, "OK");
            }
            string jsonServerResponse = JsonConvert.SerializeObject(serverResponse);
            return jsonServerResponse;
        }

        public void ServerResponse(string response)
        {
            //this is gonna be answer from server to client
            if (string.IsNullOrEmpty(response))
            {
                response = "This needs to do some status return instead!!!";
            }
            //convert response to json
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

