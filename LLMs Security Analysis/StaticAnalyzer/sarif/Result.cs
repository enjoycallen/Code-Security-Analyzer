namespace StaticAnalyzer.Sarif
{
    internal class Result
    {
        [JsonPropertyName("ruleId")]
        public string? RuleId { get; init; }

        [JsonPropertyName("ruleIndex")]
        public int RuleIndex { get; init; } = -1;

        [JsonPropertyName("rule")]
        public ReportingDescriptorReference? Rule { get; init; }

        [JsonPropertyName("level")]
        public Level Level { get; init; } = Level.warning;

        [JsonPropertyName("message"), JsonRequired]
        public required Message Message { get; init; }

        [JsonPropertyName("analysisTarget")]
        public ArtifactLocation? AnalysisTarget { get; init; }

        [JsonPropertyName("locations")]
        public List<Location> Locations { get; init; } = [];

        [JsonPropertyName("occurrenceCount")]
        public int? OccurrenceCount { get; init; }

        [JsonPropertyName("partialFingerprints")]
        public Dictionary<string, string>? PartialFingerprints { get; init; }

        [JsonPropertyName("codeFlows")]
        public List<CodeFlow> CodeFlows { get; init; } = [];

        [JsonPropertyName("relatedLocations")]
        public List<Location> RelatedLocations { get; init; } = [];

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}