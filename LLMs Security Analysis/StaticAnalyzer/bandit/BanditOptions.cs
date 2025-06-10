namespace StaticAnalyzer
{
    public class BanditOptions
    {
        public bool Recursive { get; set; }

        public Level CheckLevel { get; set; }

        public Level Confidence { get; set; }

        public ReportFormat Format { get; set; }
    }
}
