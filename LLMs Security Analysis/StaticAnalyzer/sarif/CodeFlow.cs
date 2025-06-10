namespace StaticAnalyzer.Sarif
{
    internal class CodeFlow
    {
        [JsonPropertyName("message")]
        public Message? Message { get; init; }

        [JsonPropertyName("threadFlows"), JsonRequired]
        public required List<ThreadFlow> ThreadFlows { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}