namespace StaticAnalyzer.Sarif
{
    internal class ToolComponent
    {
        [JsonPropertyName("name"), JsonRequired]
        public required string Name { get; init; }

        [JsonPropertyName("organization")]
        public string? Organization { get; init; }

        [JsonPropertyName("semanticVersion")]
        public string? SemanticVersion { get; init; }

        [JsonPropertyName("notifications")]
        public List<ReportingDescriptor> Notifications { get; init; } = [];

        [JsonPropertyName("rules")]
        public List<ReportingDescriptor> Rules { get; init; } = [];
    }
}
