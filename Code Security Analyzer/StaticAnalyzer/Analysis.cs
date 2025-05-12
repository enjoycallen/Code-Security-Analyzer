namespace StaticAnalyzer
{
    public class Analysis : IDisposable
    {
        readonly IReportGenerable _report;

        public Analysis(string source, AnalysisOptions options)
        {
            _report = options.Language switch
            {
                SupportedLanguages.C or SupportedLanguages.Cpp => new Cppcheck(
                    source: source,
                    options: new()
                    {
                        CheckLevel = options.CheckLevel ?? Level.Medium,
                        Format = options.Format,
                        Style = options.Style
                    }
                ),
                SupportedLanguages.Python => new Bandit(
                    source: source,
                    options: new()
                    {
                        Recursive = Directory.Exists(source),
                        CheckLevel = options.CheckLevel ?? Level.High,
                        Confidence = options.Confidence,
                        Format = options.Format,
                    }
                ),
                _ => throw new InvalidOperationException("不支持的语言类型")
            };
        }

        public void Print() => _report.WriteToConsoleAsync().Wait();

        public async Task PrintAsync() => await _report.WriteToConsoleAsync();

        public void Save(string filePath) => _report.SaveToFileAsync(filePath).Wait();

        public async Task SaveAsync(string filePath) => await _report.SaveToFileAsync(filePath);

        public void Dispose() => _report.Dispose();
    }
}