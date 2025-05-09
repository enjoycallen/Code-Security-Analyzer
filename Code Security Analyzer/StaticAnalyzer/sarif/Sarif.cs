namespace StaticAnalyzer.Sarif
{
    internal class Sarif
    {
        [JsonPropertyName("$schema")]
        public string? Schema { get; init; }

        [JsonPropertyName("version"), JsonRequired]
        public required string Version { get; init; }

        [JsonPropertyName("runs"), JsonRequired]
        public required List<Run>? Runs { get; init; }
    }
}