namespace StaticAnalyzer.Sarif
{
    internal class Location
    {
        [JsonPropertyName("id")]
        public int ID { get; init; } = -1;

        [JsonPropertyName("physicalLocation")]
        public PhysicalLocation PhysicalLocation { get; init; }
    }
}
