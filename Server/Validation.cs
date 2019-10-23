using Server2;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Server
{
    public static class Validation
    {

        public static Response isValidClientRequest(Request obj, CategoryManager catman)
        {

            Response builtresponse = new Response();
            var temperror = string.Empty;
            int tempstatusnum = 666;
  
            builtresponse = isValidMethodName(obj);
            tempstatusnum = builtresponse.StatusNumber;
            temperror = builtresponse.Status;

            builtresponse = isValidDate(obj);
            if (tempstatusnum == builtresponse.StatusNumber) { temperror += builtresponse.Status; }
            if (tempstatusnum != 4 && builtresponse.StatusNumber == 4) { temperror += builtresponse.Status; tempstatusnum = builtresponse.StatusNumber; }

            builtresponse = hasBody(obj);
            if (tempstatusnum == builtresponse.StatusNumber) { temperror += builtresponse.Status; }
            if (tempstatusnum != 4 && builtresponse.StatusNumber == 4) { temperror += builtresponse.Status; tempstatusnum = builtresponse.StatusNumber; }

            builtresponse = isValidPath(obj, catman);
            if (tempstatusnum == 4 && builtresponse.StatusNumber == 5) { temperror += builtresponse.Status; }
            else if (tempstatusnum != 4 && builtresponse.StatusNumber == 5) { temperror += builtresponse.Status; tempstatusnum = builtresponse.StatusNumber; }
            else if (builtresponse.Status != null) {
                if (builtresponse.Status.Equals("bad request")) { temperror = builtresponse.Status; tempstatusnum = builtresponse.StatusNumber; } }

            builtresponse = isSpecialEcho(obj);
            if (builtresponse.EchoBody != null) { tempstatusnum = builtresponse.StatusNumber; }
            builtresponse.StatusNumber = tempstatusnum;
            builtresponse.Status = temperror;

            Console.WriteLine(builtresponse.StatusNumber + " " + builtresponse.Status);
            return builtresponse;
        }
        public static Response isValidMethodName(Request obj)
        {
            var path = obj.Path;
            Response builtresponse = new Response();

            if (string.IsNullOrEmpty(path))
            {
                builtresponse.Status = " missing resource ";
                builtresponse.StatusNumber = 4;
            }
            if (string.IsNullOrEmpty(obj.Method))
            {
                Console.WriteLine("Missing method");
                builtresponse.Status += " missing method ";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
            if(!obj.Method.Equals("create") && !obj.Method.Equals("delete") && !obj.Method.Equals("read") && !obj.Method.Equals("update") && !obj.Method.Equals("echo"))
            {
                Console.WriteLine("Inside validator, illegal method");
                builtresponse.Status = " illegal method ";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }

            return builtresponse;
        }
        public static Response isValidPath(Request obj, CategoryManager catman)
        {
            var path = obj.Path;
            Response builtresponse = new Response();
 
            if (string.IsNullOrEmpty(path))
            {
                builtresponse.Status = " missing resource ";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
            string[] pathValues = Regex.Split(path, @"\/");
            int i=-1;

            foreach (string s in pathValues)
            {
                Console.WriteLine("PathValue {1} is: {0}", s, i+1);
                i++;
            }
            if (i > 0)
            {
                if (pathValues[1].Equals("api"))
                {
                    if (i > 1)
                    {
                        if (pathValues[2].Equals("categories"))
                        {
                            Console.WriteLine(pathValues[2]);
                            if (i > 2)
                            {
                                if (pathValues[3] != null)
                                    
                                {
                                    Console.WriteLine(pathValues[3]);
                                    if (obj.Method.Equals("create"))
                                    {
                                        builtresponse.Status = "bad request";
                                        builtresponse.StatusNumber = 4;
                                        return builtresponse;
                                    }
                                    else if (obj.Method.Equals("delete") || obj.Method.Equals("read") || obj.Method.Equals("update"))
                                    {
                                        try { Int32.Parse(pathValues[3]); }
                                        catch (Exception)
                                        {
                                            builtresponse.Status = "bad request";
                                            builtresponse.StatusNumber = 4;
                                            return builtresponse;
                                        }
                                        int cat = catman.GetCategoryId(Int32.Parse(pathValues[3]));
                                        if (cat == -1)
                                        {
                                            //foreach (Category catty in catman.GetCategories())
                                            //{ Console.WriteLine(catty); }
                                            Console.WriteLine("no cats found!");
                                            builtresponse.Status = "not found";
                                            builtresponse.StatusNumber = 5;
                                            return builtresponse;
                                        }
                                        else { Console.WriteLine("cats found!"); builtresponse.StatusNumber = 1; return builtresponse; }
                                        }
                                }
                            }
                            else if (obj.Method.Equals("delete") || obj.Method.Equals("update"))
                            {
                                Console.WriteLine("dele path");
                                builtresponse.Status = "bad request";
                                builtresponse.StatusNumber = 4;
                                return builtresponse;
                            }
                        }
                        else if (obj.Method.Equals("read"))
                        {
                            Console.WriteLine("dele path");
                            builtresponse.Status = "bad request";
                            builtresponse.StatusNumber = 4;
                            return builtresponse;
                        }
                    }
                    else
                    {
                        Console.WriteLine(" path");
                        builtresponse.Status = "bad request";
                        builtresponse.StatusNumber = 4;
                        return builtresponse;
                    }
                }
            }
            else
            {
                builtresponse.Status = "really bad request";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
            return builtresponse;
        }
        public static Response isValidDate(Request obj)
        {
            Response builtresponse = new Response();
            if (string.IsNullOrEmpty(obj.Date))
            {
                builtresponse.Status = " missing date ";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
            int number = 0;
            if (!Int32.TryParse(obj.Date, out number))
            {
                Console.WriteLine("Date parsed: ", number);
                builtresponse.Status = " illegal date ";
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
            return builtresponse;
        }

        public static Response hasBody(Request obj)
        {
            var method = obj.Method;
            var body = obj.Body;
            Response builtresponse = new Response();
            if (string.IsNullOrEmpty(method))
            {
                builtresponse.StatusNumber = 4;
                return builtresponse;
            }
                if(method.Equals("create") || method.Equals("echo") || method.Equals("update"))
                {
                    if (string.IsNullOrEmpty(body))
                    {
                    builtresponse.Status = " missing body ";
                    builtresponse.StatusNumber = 4;
                    return builtresponse;
                }
                }
                if (method.Equals("update"))
                {
                    try
                    {
                        //Console.WriteLine("In has body try ");
                        var canDeserialize = JsonSerializer.Deserialize<Category>(obj.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                        //Console.WriteLine(canDeserialize);
                         Console.WriteLine("can update; legal body");
                }
                    catch (Exception e)
                    {
                        Console.WriteLine("cannot update; illegal body");
                    builtresponse.Status = " illegal body ";
                    builtresponse.StatusNumber = 4;
                    return builtresponse;
                }
                }
                return builtresponse;
        }

        public static Response isSpecialEcho(Request obj)
        {
            Response builtresponse = new Response();
            if (string.IsNullOrEmpty(obj.Method))
            {
                return builtresponse;
            }
            if (obj.Method.Equals("echo") && !string.IsNullOrEmpty(obj.Body))
            {
                builtresponse.StatusNumber = 1;
                builtresponse.EchoBody = obj.Body;
                return builtresponse;
            }
            return builtresponse;
        }

        public static string GetMethodName(string name)
        {
            switch (name)
            {
                case "echo":
                    return "echo";
                case "create":
                    return "create";
                case "read":
                    return "read";
                case "update":
                    return "update";
                case "delete":
                    return "delete";
                default:
                    return "invalid";
            }

        }
       


    }
}

