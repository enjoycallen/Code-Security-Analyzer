namespace StaticAnalyzer.Sarif
{
    internal class Message
    {
        [JsonPropertyName("text")]
        public string? Text { get; init; }

        [JsonPropertyName("markdown")]
        public string? Markdown { get; init; }

        [JsonPropertyName("id")]
        public string? ID { get; init; }

        [JsonPropertyName("arguments")]
        public List<string> Arguments { get; init; } = [];

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
