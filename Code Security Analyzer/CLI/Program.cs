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

var styleOption = new Option<ReportStyle>(
    aliases: ["--style", "-s"],
    description: "报告样式"
);
styleOption.SetDefaultValue(ReportStyle.gcc);

var outputOption = new Option<string?>(
    aliases: ["--output", "-o"],
    description: "输出文件"
);

var rootCommand = new RootCommand("Codesec")
{
    sourceArgument,
    languageOption,
    checkLevelOption,
    confidenceOption,
    formatOption,
    styleOption,
    outputOption
};
rootCommand.Description = @"Code Security Analyzer
C/C++/Python 代码安全性分析工具";

rootCommand.SetHandler(static async (string source, SupportedLanguages language, Level? checkLevel, Level confidence, ReportFormat format, ReportStyle style, string? output) =>
{
    try
    {
        using var analysis = new Analysis(source, new()
        {
            Language = language,
            CheckLevel = checkLevel,
            Confidence = confidence,
            Format = format,
            Style = style,
        });

        if (output is null)
        {
            await analysis.PrintAsync();
        }
        else
        {
            if (File.Exists(output))
            {
                File.Delete(output);
            }
            await analysis.SaveAsync(output);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}, sourceArgument, languageOption, checkLevelOption, confidenceOption, formatOption, styleOption, outputOption);

await rootCommand.InvokeAsync(args);