
namespace Server
{
    public class Status
    {
        public int code { get; set; }
        public string statusBody { get; set; }

        public static string SetStatusBody(int code)
        {
            return code switch
            {
                1 => "OK",
                2 => "CREATED",
                3 => "UPDATED",
                4 => "BAD REQUEST",
                5 => "NOT FOUND",
                6 => "ERROR",
                _ => "UNKNOWN STATUS CODE",
            };
        }
    }
}
