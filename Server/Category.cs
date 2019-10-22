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
        public static List<Category> categories = new List<Category>{
                new Category {Id = 1, Name = "Beverages"},
                new Category {Id = 2, Name = "Condiments"},
                new Category {Id = 3, Name = "Confections"}
        };
    }
}