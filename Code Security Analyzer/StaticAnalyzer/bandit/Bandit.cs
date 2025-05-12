namespace StaticAnalyzer
{
    public class Bandit : IReportGenerable
    {
        static readonly string _banditPath = @"external\bandit\Scripts\bandit.exe";

        readonly ExternalCall _call;

        StreamReader IReportGenerable.Report => _call.OutputStream!;

        public Bandit(string source, BanditOptions options)
        {
            var recursive = options.Recursive ? "-r" : "";

            var checkLevel = options.CheckLevel switch
            {
                Level.Low => "--severity high",
                Level.Medium => "--severity medium",
                Level.High => "",
                _ => throw new ArgumentException("severity 参数错误")
            };

            var confidence = options.Confidence switch
            {
                Level.Low => "",
                Level.Medium => "--confidence medium",
                Level.High => "--confidence high",
                _ => throw new ArgumentException("confidence 参数错误")
            };

            var format = options.Format switch
            {
                ReportFormat.csv => "-f csv",
                ReportFormat.html => "-f html",
                ReportFormat.json => "-f json",
                ReportFormat.txt => "-f txt",
                ReportFormat.xml => "-f xml",
                ReportFormat.yaml => "-f yaml",
                _ => throw new UnsupportedReportFormatException("bandit", options.Format)
            };

            _call = new(
                path: _banditPath,
                args: $"{source} {recursive} {checkLevel} {confidence} {format}",
                option: RedirectOption.StandardOutput
            );
        }

        public void Terminate() => _call.Terminate();

        public void Dispose() => _call.Dispose();
    }
}