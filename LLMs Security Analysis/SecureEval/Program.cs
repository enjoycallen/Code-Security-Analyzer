using SecureEval;
using System.CommandLine;

var rootCommand = new RootCommand("SecureEval")
{
    DatasetCommand.RootCommand
};

try
{
    OllamaClient.Init();
    //CweInfo.Load();

    await rootCommand.InvokeAsync(args);

    OllamaClient.Close();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}