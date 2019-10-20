using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Response
    {
        public int status { get; set; }
        public string body { get; set; }

        public Response()
        {

        }
        public Response(int status, string body)
        {
            this.status = status;
            this.body = body;
        }
    }
}
