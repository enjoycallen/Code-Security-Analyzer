namespace StaticAnalyzer.Sarif
{
    internal class Artifact
    {
        [JsonPropertyName("description")]
        public Message? Desciption { get; init; }

        [JsonPropertyName("location")]
        public ArtifactLocation? Location { get; init; }
    }
}
