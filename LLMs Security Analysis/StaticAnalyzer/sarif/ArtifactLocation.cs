namespace StaticAnalyzer.Sarif
{
    internal class ArtifactLocation
    {
        [JsonPropertyName("uri")]
        public string? Uri { get; init; }

        [JsonPropertyName("uriBaseId")]
        public string? UriBaseId { get; init; }

        [JsonPropertyName("index")]
        public int Index { get; init; } = -1;

        [JsonPropertyName("description")]
        public Message? Description { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
