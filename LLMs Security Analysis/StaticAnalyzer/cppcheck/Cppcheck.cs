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

            _call = new(
                path: _cppcheckPath,
                args: $"{source} {checkLevel} {format} --check-level=exhaustive",
                option: RedirectOption.StandardError
            );
        }

        public void WaitForExit() => _call.WaitForExit();

        public async Task WaitForExitAsync() => await _call.WaitForExitAsync();

        public void Terminate() => _call.Terminate();

        public void Dispose() => _call.Dispose();
    }
}