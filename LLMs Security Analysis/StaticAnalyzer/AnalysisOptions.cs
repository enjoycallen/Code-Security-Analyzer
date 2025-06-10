namespace StaticAnalyzer
{
    public class AnalysisOptions
    {
        public SupportedLanguages Language { get; set; }

        public List<Tools>? Tools { get; set; }

        public Level? CheckLevel { get; set; }

        public Level Confidence { get; set; }

        public ReportFormat Format { get; set; }
    }

    public enum SupportedLanguages
    {
        C,
        Cpp,
        Python,
    }

    public enum Level
    {
        Low,
        Medium,
        High
    }

    public enum Tools
    {
        Cppcheck,
        Bandit,
        Codeql
    }
}