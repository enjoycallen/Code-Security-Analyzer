namespace StaticAnalyzer.Sarif
{
    internal class ThreadFlow
    {
        [JsonPropertyName("id")]
        public string? ID { get; init; }

        [JsonPropertyName("message")]
        public Message? Message { get; init; }

        [JsonPropertyName("locations"), JsonRequired]
        public required List<ThreadFlowLocation> Locations { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
