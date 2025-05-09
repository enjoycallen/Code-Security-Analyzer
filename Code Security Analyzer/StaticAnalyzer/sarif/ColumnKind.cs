namespace StaticAnalyzer.Sarif
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum ColumnKind
    {
        utf16CodeUnits,
        unicodeCodePoints
    }
}