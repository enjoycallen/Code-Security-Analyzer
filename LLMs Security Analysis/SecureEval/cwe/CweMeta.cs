using System.Text.Json.Serialization;

namespace SecureEval
{
    internal class CweMeta
    {
        public class Weakness
        {
            [JsonPropertyName("id")]
            public string ID { get; init; } = "";

            [JsonPropertyName("name")]
            public string Name { get; init; } = "";

            [JsonPropertyName("description")]
            public string Description { get; init; } = "";

            [JsonPropertyName("parents")]
            public List<string> Parents { get; init; } = [];

            [JsonPropertyName("peers")]
            public List<string> Peers { get; init; } = [];

            [JsonPropertyName("categories")]
            public List<string> Categories { get; init; } = [];
        }

        public class Category
        {
            [JsonPropertyName("id")]
            public string ID { get; init; } = "";

            [JsonPropertyName("summary")]
            public string Summary { get; init; } = "";
        }

        [JsonPropertyName("weaknesses")]
        public List<Weakness> Weaknesses { get; init; } = [];

        [JsonPropertyName("categories")]
        public List<Category> Categories { get; init; } = [];
    }
}
