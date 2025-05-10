namespace StaticAnalyzer
{
    public class UnsupportedReportFormatException(string analyzer, ReportFormat format)
        : Exception($"{analyzer}: error: 不支持生成 {format} 格式的分析报告。");
}
