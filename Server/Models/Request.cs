using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Request
    {
        // a correct reponse from the user needs to have a  method,  a path, a date and  a body of different formats;
        public string Method { get; set; }
        public string Path { get; set; }
        public long Date { get; set; }
        public string Body { get; set; }
    
    }
}
