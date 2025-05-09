namespace StaticAnalyzer.Sarif
{
    internal class ReportingConfiguration
    {
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; init; }

        [JsonPropertyName("level")]
        public Level Level { get; init; } = Level.warning;

        [JsonPropertyName("rank")]
        public double? Rank { get; init; }

        [JsonPropertyName("parameters")]
        public PropertyBag? Parameters;

        [JsonPropertyName("properties")]
        public PropertyBag? Properties;
    }
}
