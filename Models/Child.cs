using System.Text.Json.Serialization;

namespace Models
{
    public class Child
    {
        public string Name { get; set; }

        public Parent? Parent { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string SomeString { get; set; } = "SomeString";
    }
}