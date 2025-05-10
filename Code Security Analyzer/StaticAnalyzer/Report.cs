namespace StaticAnalyzer
{
    public interface IReportGenerable
    {
        protected Stream ReportStream { get; }

        async Task WriteToStreamAsync(Stream stream)
        {
            await ReportStream.CopyToAsync(stream);
        }

        async Task SaveToFileAsync(string filePath)
        {
            var stream = File.OpenWrite(filePath);
            await WriteToStreamAsync(stream);
            stream.Close();
        }

        async Task WriteToConsoleAsync()
        {
            await WriteToStreamAsync(Console.OpenStandardOutput());
        }
    }
}