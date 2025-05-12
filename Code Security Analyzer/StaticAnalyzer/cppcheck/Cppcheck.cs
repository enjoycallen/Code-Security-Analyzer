namespace StaticAnalyzer
{
    public sealed class Cppcheck : IDisposable, IReportGenerable
    {
        static readonly string _cppcheckPath = @"external\cppcheck\cppcheck.exe";

        readonly ExternalCall _call;

        StreamReader IReportGenerable.Report => _call.ErrorStream!;

        public Cppcheck(string source, CppcheckOptions options)
        {
            var checkLevel = options.CheckLevel switch
            {
                Level.Low => "",
                Level.Medium => "--enable=warning",
                Level.High => "--enable=all",
                _ => throw new ArgumentException("severity 参数错误")
            };

            var format = options.Format switch
            {
                ReportFormat.txt => "",
                ReportFormat.xml => "--output-format=xml",
                ReportFormat.sarif => "--output-format=sarif",
                _ => throw new UnsupportedReportFormatException("cppcheck", options.Format)
            };

            var style = options.Format != ReportFormat.txt ? "" : "--template=" +
                options.Style switch
                {
                    ReportStyle.gcc => "gcc",
                    ReportStyle.cppcheck1 => "cppcheck1",
                    ReportStyle.vs => "vs",
                    ReportStyle.edit => "edit",
                    _ => throw new ArgumentException("style 参数错误")
                };

            _call = new(
                path: _cppcheckPath,
                args: $"{source} {checkLevel} {format} {style}",
                option: RedirectOption.StandardError
            );
        }

        public void WaitForExit() => _call.WaitForExit();

        public async Task WaitForExitAsync() => await _call.WaitForExitAsync();

        public void Terminate() => _call.Terminate();

        public void Dispose() => _call.Dispose();
    }
}