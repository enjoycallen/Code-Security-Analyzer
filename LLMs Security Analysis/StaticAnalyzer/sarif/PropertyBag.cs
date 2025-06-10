namespace StaticAnalyzer.Sarif
{
    internal class PropertyBag
    {
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = [];
    }
}
