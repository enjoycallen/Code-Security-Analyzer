namespace StaticAnalyzer.Sarif
{
    internal class Notification
    {
        [JsonPropertyName("locations")]
        public List<Location> Locations { get; init; } = [];

        [JsonPropertyName("message"), JsonRequired]
        public required Message Message { get; init; }

        [JsonPropertyName("level")]
        public Level Level { get; init; } = Level.warning;

        [JsonPropertyName("timeUtc")]
        public string? TimeUtc { get; init; }

        [JsonPropertyName("descriptor")]
        public ReportingDescriptorReference? Descriptor { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
