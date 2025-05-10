using StaticAnalyzer;
using System.CommandLine;

var sourceArgument = new Argument<string>(
    name: "source",
    description: "源程序路径"
);

var levelOption = new Option<CheckLevel>(
    name: "--level",
    description: "检查级别"
);
levelOption.SetDefaultValue(CheckLevel.Error);

var formatOption = new Option<ReportFormat>(
    name: "--format",
    description: "报告格式"
);
formatOption.SetDefaultValue(ReportFormat.Text);

var styleOption = new Option<ReportStyle>(
    name: "--style",
    description: "报告样式"
);
styleOption.SetDefaultValue(ReportStyle.Gcc);

var outputOption = new Option<string?>(
    name: "--output",
    description: "输出文件"
);

var rootCommand = new RootCommand("Codesec")
{
    sourceArgument,
    levelOption,
    formatOption,
    styleOption,
    outputOption
};

rootCommand.SetHandler(static async (string source, CheckLevel level, ReportFormat format, ReportStyle style,string? output) =>
{
    /*
    Console.WriteLine($"源程序路径：{source}");
    Console.WriteLine($"检查级别：{level}");
    Console.WriteLine($"报告格式：{format}");
    Console.WriteLine($"报告样式：{style}");
    Console.WriteLine($"输出文件：{output}");
    */

    try
    {
        using var analysis = new Cppcheck(source, new()
        {
            Level = level,
            Format = format,
            Style = style,
        });

        IReportGenerable report = analysis;
        await (output is null ? report.WriteToConsoleAsync() : report.SaveToFileAsync(output));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}, sourceArgument, levelOption, formatOption, styleOption,outputOption);

await rootCommand.InvokeAsync(args);