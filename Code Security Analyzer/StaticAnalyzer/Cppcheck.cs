using System.Runtime.CompilerServices;

namespace StaticAnalyzer
{
    public sealed class Cppcheck : IDisposable, IReportGenerable
    {
        static readonly string _cppcheckPath = @"external\cppcheck\cppcheck.exe";

        readonly ExternalCall _call;

        public Stream ReportStream => _call.ErrorStream!;

        public Cppcheck(string path, CppcheckOptions options)
        {
            var level = options.Level switch
                {
                    CheckLevel.Error => "",
                    CheckLevel.Warning => "--enable=warning",
                    _ => throw new ArgumentException("level 参数错误")
                };

            var format = options.Format switch
            {
                ReportFormat.Text => "",
                ReportFormat.Xml => "--output-format=xml",
                ReportFormat.Sarif => "--output-format=sarif",
                _ => throw new UnsupportedReportFormatException("cppcheck", options.Format)
            };

            var style = options.Format != ReportFormat.Text ? "" : "--template=" +
                options.Style switch
                {
                    ReportStyle.Gcc => "gcc",
                    ReportStyle.Cppcheck1 => "cppcheck1",
                    ReportStyle.Vs => "vs",
                    ReportStyle.Edit => "edit",
                    _ => throw new ArgumentException("style 参数错误")
                };

            _call = new(
                path: _cppcheckPath,
                args: $"{path} {level} {format} {style}",
                option: RedirectOption.StandardError
            );
        }

        public void WaitForExit() => _call.WaitForExit();

        public async Task WaitForExitAsync() => await _call.WaitForExitAsync();

        public void Terminate() => _call.Terminate();

        public void Dispose() => _call.Dispose();
    }
}