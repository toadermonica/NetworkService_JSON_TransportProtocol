using System;
using System.Text;
using System.Net.Sockets;
using Server.Helpers;
using Server.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server
{
    public class ServerController
    {
        protected TcpClient clientInstance { get; set; }
        protected NetworkStream stream { get; set; }

        public ServerController()
        {

        }
        public ServerController(TcpClient clientInstance, NetworkStream stream)
        {
            this.clientInstance = clientInstance;
            this.stream = stream;
        }


        public void HandleOpperation(TcpClient clientInstance)
        {
            stream = clientInstance.GetStream();
            byte[] buffer = new byte[clientInstance.ReceiveBufferSize];
            var length = buffer.Length;
            var rcnt = stream.Read(buffer, 0, length);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, rcnt);
            ServerResponse(receivedMessage);
        }

        public void ServerResponse(string receivedMessage)
        {
            var value = receivedMessage;
            Request request = JsonSerializer.Deserialize<Request>(receivedMessage);
            Service service = new Service(request);
            Response response;
            if (request.Method.Equals("echo"))
            {
                var body = request.Body;
                response = service.Echo(body);
                var jsonServerResponse = JsonSerializer.Serialize(request);
                var msg = Encoding.UTF8.GetBytes(jsonServerResponse);
                stream.Write(msg, 0, msg.Length);                           
                stream.Close();
            }
        }

    }
}

