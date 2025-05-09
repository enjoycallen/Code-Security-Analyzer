namespace StaticAnalyzer.Sarif
{
    internal class ReportingDescriptorReference
    {
        [JsonPropertyName("id")]
        public string? ID { get; init; }

        [JsonPropertyName("index")]
        public int Index { get; init; } = -1;

        [JsonPropertyName("guid")]
        public string? Guid { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
