using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Request
    {
        // a correct reponse from the user needs to have a  method,  a path, a date and  a body of different formats;
        public string method { get; set; }
        public string path { get; set; }
        public long date { get; set; }
        public string body { get; set; }
    
    }
}
