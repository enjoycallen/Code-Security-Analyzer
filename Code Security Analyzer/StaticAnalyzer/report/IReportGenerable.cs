using System.Reflection.PortableExecutable;

namespace StaticAnalyzer
{
    internal interface IReportGenerable : IDisposable
    {
        protected StreamReader Report { get; }

        async Task WriteToConsoleAsync()
        {
            await Report.BaseStream.CopyToAsync(Console.OpenStandardOutput());
        }

        async Task SaveToFileAsync(string filePath)
        {
            using var file = new StreamWriter(filePath);
            char[] buffer = new char[8192];

            int bytesRead;
            while ((bytesRead = await Report.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await file.WriteAsync(buffer, 0, bytesRead);
            }
        }
    }
}