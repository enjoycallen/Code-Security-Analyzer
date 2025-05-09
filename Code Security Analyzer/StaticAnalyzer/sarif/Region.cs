namespace StaticAnalyzer.Sarif
{
    internal class Region
    {
        [JsonPropertyName("startLine")]
        public int? StartLine { get; init; }

        [JsonPropertyName("startColumn")]
        public int? StartColumn { get; init; }

        [JsonPropertyName("endLine")]
        public int? EndLine { get; init; }

        [JsonPropertyName("endColumn")]
        public int? EndColumn { get; init; }

        [JsonPropertyName("message")]
        public Message? Message { get; init; }

        [JsonPropertyName("sourceLanguage")]
        public string? SourceLanguage { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? properties;
    }
}
