using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Server
{
    public static class Validation
    {

        public static bool isValidClientRequest(Request obj, out string error, out string specialBody)
        {
            error = string.Empty;
            specialBody = null;
            var finalError = string.Empty;
            // the ? : conditional operator "if isValidMethod then return string.empty else return the out parameter of the function"
            finalError += Validation.isValidMethodName(obj, out string methodError) ? string.Empty : methodError;
            finalError += Validation.isValidPath(obj, out string pathError) ? string.Empty : pathError;
            finalError += Validation.isValidDate(obj, out string dateError) ? string.Empty : dateError;
            finalError += Validation.hasBody(obj, out string bodyError) ? string.Empty : bodyError;
            specialBody += Validation.isSpecialEcho(obj, out string specialReturn) ? specialReturn : null;
            if (string.IsNullOrEmpty(finalError))
            {
                //request parsed through this mother function is valid (so far)
                //also return the cumulated error stings in the error out parameter
                error += finalError;
                return true;
            }
            //the finalError parameter is not an empty string -> add it to out param error and then return false;
            error += finalError;
            return false;
        }
        public static bool isValidMethodName(Request obj, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(obj.Method))
            {
                Console.WriteLine("Missing method");
                error = " missing method ";
                return false;
            }
            if(!obj.Method.Equals("create") && !obj.Method.Equals("delete") && !obj.Method.Equals("read") && !obj.Method.Equals("update") && !obj.Method.Equals("echo"))
            {
                Console.WriteLine("Inside validator, illegal method");
                error = " illegal method ";
                return false;
            }
            return true;
        }
        public static bool isValidPath(Request obj, out string error)
        {
            var path = obj.Path;
            error = string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                error = " missing resource ";
                return false;
            }
            /*if(path.Contains("/api/") && )*/
            
            return true;
        }
        public static bool isValidDate(Request obj, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(obj.Date))
            {
                error = " missing date ";
                return false;
            }
            int number = 0;
            if (!Int32.TryParse(obj.Date, out number))
            {
                Console.WriteLine("Number parsed: ", number);
                error = " illegal date ";
                return false;
            }
            
            return true;
        }

        public static bool hasBody(Request obj, out string bodyError)
        {
            var method = obj.Method;
            var body = obj.Body;

            if (string.IsNullOrEmpty(method))
            {
                bodyError = string.Empty;
                return true;
            }
            if(!method.Equals("delete") && !method.Equals("read"))
            {
                if(method.Equals("create") || method.Equals("echo") || method.Equals("update"))
                {
                    if (string.IsNullOrEmpty(body))
                    {
                        bodyError = " missing body ";
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
                    catch (Exception e)
                    {
                        Console.WriteLine("In the exception");
                        bodyError = " illegal body ";
                        return false;
                    }
                }

            }
            
            bodyError = string.Empty;
            return true;
        }

        public static bool isSpecialEcho(Request obj, out string specialReturn)
        {

            if (string.IsNullOrEmpty(obj.Method))
            {
                specialReturn = null;
                return false;
            }
            if (obj.Method.Equals("echo") && !string.IsNullOrEmpty(obj.Body))
            {
                specialReturn = obj.Body;
                return true;
            }
            specialReturn = null;
            return false;
        }

        
    }
}