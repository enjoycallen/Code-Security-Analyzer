namespace StaticAnalyzer.Sarif
{
    internal class ThreadFlowLocation
    {
        [JsonPropertyName("index")]
        public int Index { get; init; } = -1;

        [JsonPropertyName("location")]
        public Location? Location { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}