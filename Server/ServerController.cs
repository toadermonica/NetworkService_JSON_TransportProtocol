using System;
using System.Text;
using System.Net.Sockets;
using Server.Helpers;
using Server.Services;

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
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, rcnt);
            return receivedMessage;
        }

        public void ServerResponse(string receivedMessage)
        {
            string response = SetResponse(receivedMessage);
            //response = response.ToUpper();                              //- need a server message/response to be set here
            byte[] msg = Encoding.ASCII.GetBytes(response);             // make the message into bytes again and encode
            stream.Write(msg, 0, msg.Length);                           // add/write to network stream for client
            stream.Close();                                             //close stream with client
        }
        private string SetResponse(string receivedMessage)
        {

            Request request = JsonConvert.DeserializeObject<Request>(receivedMessage);
            Service service = new Service(request);
            var response = service.ExecuteMethod(request);
            string jsonServerResponse = JsonConvert.SerializeObject(response);
            return jsonServerResponse;


        }

    }
}

