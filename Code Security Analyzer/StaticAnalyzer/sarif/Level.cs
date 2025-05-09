namespace StaticAnalyzer.Sarif
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Level
    {
        none,
        note,
        warning,
        error
    }
}
