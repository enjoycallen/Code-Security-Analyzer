namespace StaticAnalyzer.Sarif
{
    internal class MultiformatMessageString
    {
        [JsonPropertyName("text"), JsonRequired]
        public required string Text { get; init; }

        [JsonPropertyName("markdown")]
        public string? Markdown { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
