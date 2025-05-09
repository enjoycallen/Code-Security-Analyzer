namespace StaticAnalyzer.Sarif
{
    internal class Invocation
    {
        [JsonPropertyName("toolExecutionNotifications")]
        public List<Notification> ToolExecutionNotifications { get; init; } = [];

        [JsonPropertyName("executionSuccessful"), JsonRequired]
        public required bool ExecutionSuccessful { get; init; }
    }
}
