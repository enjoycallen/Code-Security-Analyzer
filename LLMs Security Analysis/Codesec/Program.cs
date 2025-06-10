using StaticAnalyzer;
using System.CommandLine;

var sourceArgument = new Argument<string>(
    name: "source",
    description: "源程序路径"
);

var languageOption = new Option<SupportedLanguages>(
    aliases: ["--language"],
    description: "源程序语言"
)
{
    IsRequired = true
};

var toolsOption = new Option<List<Tools>>(
    aliases: ["--tools"],
    description: "分析工具"
);

var checkLevelOption = new Option<Level?>(
    aliases: ["--level", "-l"],
    description: "检查级别"
);

var confidenceOption = new Option<Level>(
    aliases: ["--confidence", "-c"],
    description: "置信度"
);
confidenceOption.SetDefaultValue(Level.Low);

var formatOption = new Option<ReportFormat>(
    aliases: ["--format", "-f"],
    description: "报告格式"
);
formatOption.SetDefaultValue(ReportFormat.txt);

var outputOption = new Option<string?>(
    aliases: ["--output", "-o"],
    description: "输出文件"
);

var rootCommand = new RootCommand("Codesec")
{
    sourceArgument,
    languageOption,
    toolsOption,
    checkLevelOption,
    confidenceOption,
    formatOption,
    outputOption
};
rootCommand.Description = @"Code Security Analyzer
C/C++/Python 代码安全性分析工具";

rootCommand.SetHandler(static async (string source, SupportedLanguages language, List<Tools> tools, Level? checkLevel, Level confidence, ReportFormat format, string? output) =>
{
    try
    {
        using var analysis = new Analysis(source, new()
        {
            Language = language,
            Tools = tools,
            CheckLevel = checkLevel,
            Confidence = confidence,
            Format = format,
        });
        await (output is null ? analysis.PrintAsync() : analysis.SaveAsync(output));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}, sourceArgument, languageOption, toolsOption, checkLevelOption, confidenceOption, formatOption, outputOption);

await rootCommand.InvokeAsync(args);