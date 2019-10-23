using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Server
{
    public class Category
    {
        [JsonPropertyName("cid")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + Name;
        }
    }
}