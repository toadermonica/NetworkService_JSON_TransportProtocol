using System;
using System.Text.Json;

namespace Server
{
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
                try
                {
                    Console.WriteLine("In has body try ");
                    var canDeserialize = JsonSerializer.Deserialize<Category>(obj.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    Console.WriteLine(canDeserialize);
                }
                catch(Exception e){
                    Console.WriteLine("In the exception");
                    bodyError = "illegal body";
                    return false;
                }
            }
            bodyError = string.Empty;
            return true;
        }

    }
}