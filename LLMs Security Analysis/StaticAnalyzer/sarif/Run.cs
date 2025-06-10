namespace StaticAnalyzer.Sarif
{
    internal class Run
    {
        [JsonPropertyName("tool"), JsonRequired]
        public required Tool Tool { get; init; }

        [JsonPropertyName("invocations")]
        public List<Invocation> Invocations { get; init; } = [];

        [JsonPropertyName("language")]
        public string Language { get; init; } = "en-US";

        [JsonPropertyName("artifacts")]
        public List<Artifact>? Artifacts { get; init; }

        [JsonPropertyName("results")]
        public List<Result>? Results { get; init; }

        [JsonPropertyName("columnKind")]
        public ColumnKind? ColumnKind { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
