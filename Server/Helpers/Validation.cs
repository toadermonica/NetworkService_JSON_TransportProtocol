using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Helpers
{
    class Validation
    {
        public string requestMessage { get; set; }
        public Validation(string requestMessage)
        {
            this.requestMessage = requestMessage;
        }

        //Will take care of deserialization of json request 
        //and parse to object of type request and category depending on the case
        public bool isValidRequest(out string error)
        {
            Request request;
            error = string.Empty;

            //check if request from client is of type JSON object
            try
            {
                request = JsonConvert.DeserializeObject<Request>(this.requestMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                error = "invlaid JSON";
                return false;
            }

            error += isValidMethod(request.method) + isValidResource(request.path) + isValidDate(request.date) + isValidBody(request.body);

            //if all is fine then return true and error will be empty
            return true;
        }

        private string isValidMethod(string method)
        {
            string error = string.Empty;
            //check if method provided is valid
            if (method == null)
            {
                error = "missing method";
                return error;
            }

            if (!method.Equals("create") && !method.Equals("read") && !method.Equals("update") && !method.Equals("delete") && !method.Equals("echo"))
            {
                error = "illegal method";
                return error;
            }
            return error;
        }
        private string isValidDate(long date)
        {
            string error = string.Empty;
            //check if date provided is valid
            var requestDate = date;
            if (string.IsNullOrEmpty(requestDate.ToString()))
            {
                error = "missing date";
                return error;
            }
            var isLong = requestDate.GetType().Equals(typeof(long));
            if (!isLong)
            {
                error = "illegal date";
                return error;
            }
            return error;
        }
        private string isValidResource(string path)
        {
            string error = string.Empty;
            //check if resource is valid
            var requestResource = path;
            if (string.IsNullOrEmpty(requestResource))
            {
                error = "missing resource";
                return error;
            }
            return error;
        }
        private string isValidBody(string body)
        {
            Category requestBody;

            var error = string.Empty;
            //check if body provided is valid
            if (string.IsNullOrEmpty(body))
            {
                error = "missing body";
                return error;
            }

            try
            {
                //try and deserialize the string to json object and if it is not json then catch error
                requestBody = JsonConvert.DeserializeObject<Category>(body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                error = "illegal body";
                return error;
            }

            return error;

           /* var bodyCid = requestBody.cid;
            var bodyName = requestBody.name;
            if (string.IsNullOrEmpty(bodyCid.ToString()))
            {
                error = "missing cid";
                return error;
            }
            if (string.IsNullOrEmpty(bodyName))
            {
                error = "missing bodyName";
                return error;
            }*/
        }


    }
}
