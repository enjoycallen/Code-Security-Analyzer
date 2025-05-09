namespace StaticAnalyzer.Sarif
{
    internal class ReportingDescriptor
    {
        [JsonPropertyName("id"), JsonRequired]
        public required string ID { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("shortDescription")]
        public MultiformatMessageString? ShortDescription { get; init; }

        [JsonPropertyName("fullDescription")]
        public MultiformatMessageString? FullDescription { get; init; }

        [JsonPropertyName("defaultConfiguration")]
        public ReportingConfiguration? DefaultConfiguration { get; init; }

        [JsonPropertyName("properties")]
        public PropertyBag? Properties;
    }
}