
namespace Server.Services
{
    public static class Service
    {
        private const string defaultPath = "/api/categories";

        


        /* public Response APIResponse()
         {
             Validation validator = new Validation();
             bool validMethod = string.IsNullOrEmpty(validator.isValidMethod(requestMessage.Method));
             if (validMethod)
             {
                 var response = ExecuteMethod();
                 return response;
             }
             else
             {
                 var response = ExecuteMethod();
                 return response;
             }
         }*/

        /*public Response ExecuteMethod(Request requestMessage)
        {
            Validation validator = new Validation();
            string method = requestMessage.Method;
            string path = requestMessage.Path;
            if (method.Equals("create") && path.Equals(defaultPath))
            {
                return Echo();
            }
            if (method.Equals("read"))
            {
                return Echo();
            }
            if (method.Equals("update"))
            {
                return Echo();
            }
            if (method.Equals("delete"))
            {
                return Echo();
            }
            if (validator.isInvalid(requestMessage, out string error))
            {
                return ErrorResponse(error);
            }
            return Echo();
        }*/

       /* public void Create(string path)
        {
            *//*New elements can be added by use of the path and the new element in the body of the
            request. Using path and an id is invalid and should return “4 Bad request”. On successful
            creation return the “2 Created” status plus the newly create element in the body*//*
        }
        public void Read(string path)
        {
            *//*The list of all elements can be retrieved by use of the path, individual elements can be
              retrieved by adding the “/<id>” to the path (the id of the element to be retrieved without the
              <>). Requesting individual elements must return status “5 Not found” if the requested element
              is not in the database, otherwise the status is “1 Ok”.*//*
        }
        public void Update(string path)
        {
            *//*All elements can be updated by use of the path extended with the id and the updated element
                in the body. Updates without an id in the path is not allowed and should return “4 Bad
                request”. On successful update return the “3 Updated” status.*//*
        }
        public void Delete(string path)
        {
            *//*All elements can be deleted by use of the path extended with the id. If the element is not in
            the database “5 Not found” should be returned otherwise “1 Ok”*//*
        }*/

        /*This method does not take any path, it is just ignored if provided, and will simply return the
            body of the request as the body of the response with status “1 Ok”. */
       

    }
}
