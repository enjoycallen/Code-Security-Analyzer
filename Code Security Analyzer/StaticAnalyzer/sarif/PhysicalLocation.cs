namespace StaticAnalyzer.Sarif
{
    internal class PhysicalLocation
    {
        [JsonPropertyName("artifactLocation")]
        public ArtifactLocation? ArtifactLocation { get; init; }

        [JsonPropertyName("region")]
        public Region? Region { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties { get; init; }
    }
}
