namespace StaticAnalyzer.Sarif
{
    internal class Tool
    {
        [JsonPropertyName("driver"), JsonRequired]
        public required ToolComponent Driver { get; init; }

        [JsonPropertyName("extensions")]
        public List<ToolComponent> Extensions { get; init; } = [];
    }
}