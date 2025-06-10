using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;

namespace SecureEval
{
    internal static class DatasetCommand
    {
        #region 选项
        static readonly Option<Languages> _languageOption = new(
            aliases: ["--language", "-l"],
            description: "语言"
        )
        {
            IsRequired = true
        };

        static readonly Option<Models> _modelOption = new(
            aliases: ["--model", "-m"],
            description: "模型"
        )
        {
            IsRequired = true
        };

        static readonly Option<string> _outputRootOption = new(
            aliases: ["--output-root", "-o"],
            description: "输出目录"
        )
        {
            IsRequired = true
        };

        static readonly Option<string> _inputRootOption = new(
            aliases: ["--input-root", "-i"],
            description: "数据集根目录"
        )
        {
            IsRequired = true
        };

        static readonly Option<List<Models>> _evalModelOption = new(
            aliases: ["--eval-model", "-m"],
            description: "评估模型"
        )
        {
            AllowMultipleArgumentsPerToken = true
        };
        #endregion

        #region 子命令
        static readonly Command _createCommand = new("create", "创建数据集")
        {
            _languageOption,
            _modelOption,
            _outputRootOption
        };

        static Command _evalCommand = new("eval", "评估数据集")
        {
            _inputRootOption,
            _evalModelOption
        };

        static Command _analyzeCommand = new("analyze", "分析数据集")
        {
            _inputRootOption
        };
        #endregion

        ///根命令
        public static Command RootCommand = new("dataset", "数据集相关命令")
        {
            _createCommand,
            _evalCommand,
            _analyzeCommand
        };

        static DatasetCommand()
        {
            _createCommand.SetHandler(CreateDataset, _languageOption, _modelOption, _outputRootOption);
            _evalCommand.SetHandler(EvalDataset, _inputRootOption, _evalModelOption);
            _analyzeCommand.SetHandler(AnalyzeDataset, _inputRootOption);
        }

        static async Task CreateDataset(Languages languageArg, Models modelArg, string outputRootArg)
        {
            try
            {
                #region 解析参数
                ModelBase model = modelArg switch
                {
                    Models.Codegeex4 => new Codegeex4(),
                    Models.Codellama => new Codellama(),
                    Models.DeepseekCoderV2 => new DeepseekCoderV2(),
                    Models.DeepseekR1 => new DeepseekR1(),
                    Models.Gemma3 => new Gemma3(),
                    Models.Llama3 => new Llama3(),
                    Models.Mistral => new Mistral(),
                    Models.DeepCoderV2 => new DeepCoderV2(),
                    _ => throw new NotImplementedException($"不支持 {modelArg} 模型。")
                };

                var lang = languageArg switch
                {
                    Languages.C => "C",
                    Languages.Cpp => "Cpp",
                    Languages.Python => "Python",
                    _ => throw new NotImplementedException($"不支持 {languageArg} 语言。")
                };
                var ext = languageArg switch
                {
                    Languages.C => "c",
                    Languages.Cpp => "cpp",
                    Languages.Python => "py",
                    _ => throw new NotImplementedException($"不支持 {languageArg} 语言。")
                };
                #endregion

                //解析json文件
                using var file = File.OpenRead(@"dataset\tasks.json");
                var doc = await JsonDocument.ParseAsync(file);
                var tasks = doc.RootElement.GetProperty(languageArg.ToString()).Deserialize<List<string>>() ?? [];

                //创建数据集
                Console.WriteLine(@$"开始创建数据集 testcases_{modelArg}\{languageArg} ...");
                int index = 0;
                foreach (var task in tasks)
                {
                    var dir = $@"{outputRootArg}\testcases_{modelArg}\{lang}\testcase_{index}";
                    var source = @$"{dir}\src.{ext}";

                    Console.WriteLine($"进度：{++index}/{tasks.Count}");
                    Console.WriteLine(@$"正在生成{source}...");
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    var code = await model.GenerateCodeAsync(languageArg, task);
                    stopwatch.Stop();
                    Console.WriteLine(@$"用时 [{Math.Round(stopwatch.ElapsedMilliseconds / 1000.0, 1)}s]");

                    Directory.CreateDirectory(dir);
                    await File.WriteAllTextAsync(source, code);
                    
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("创建数据集失败。", ex);
            }
        }

        static async Task EvalDataset(string inputRootArg, List<Models> evalModelArg)
        {
            try
            {
                if (!Directory.Exists(inputRootArg))
                {
                    throw new InvalidOperationException("数据集目录不存在。");
                }
                var testcasesDir = Directory.GetDirectories(inputRootArg);

                if (evalModelArg.Count == 0)
                {
                    //人工评估数据集
                    Console.WriteLine(@$"开始评估数据集 {inputRootArg} ......");
                    foreach (var dir in testcasesDir)
                    {
                        var files = Directory.GetFiles(dir, "src.*");
                        foreach (var file in files)
                        {
                            var name = Path.GetFileName(file);
                            var code = await File.ReadAllTextAsync(file);

                            Console.WriteLine($"正在评估 {file}\n{name}:\n{code}");
                            Console.Write("输入 CWE 标签：");

                            var label = Console.ReadLine();
                            if (label == "") label = null;
                            else label = $"CWE-{label}";
                            var result = new
                            {
                                src = name,
                                label
                            };

                            await File.WriteAllTextAsync($@"{dir}\meta.json", result.ToJsonString());
                        }
                    }
                }
                else
                {
                    List<ModelBase> models = [];
                    foreach (var model in evalModelArg)
                    {
                        ModelBase modelBase = model switch
                        {
                            Models.Codegeex4 => new Codegeex4(),
                            Models.Codellama => new Codellama(),
                            Models.DeepseekCoderV2 => new DeepseekCoderV2(),
                            Models.DeepseekR1 => new DeepseekR1(),
                            Models.Gemma3 => new Gemma3(),
                            Models.Llama3 => new Llama3(),
                            Models.Mistral => new Mistral(),
                            Models.DeepCoderV2 => new DeepCoderV2(),
                            _ => throw new NotImplementedException($"不支持 {model} 模型。")
                        };
                        models.Add(modelBase);
                    }

                    //LLMs 评估数据集
                    Console.WriteLine(@$"开始评估数据集 {inputRootArg} ......");
                    foreach (var dir in testcasesDir)
                    {
                        var files = Directory.GetFiles(dir, "src.*");
                        foreach (var file in files)
                        {
                            Console.WriteLine(@$"正在评估 {file} ...");
                            Stopwatch stopwatch = Stopwatch.StartNew();

                            var code = await File.ReadAllTextAsync(file);
                            EvalResult eval = new();
                            foreach (var model in models)
                            {
                                var res = await model.EvalDatasetAsync(code, 1.0 / models.Count);
                                eval.Update(res);
                            }

                            var result = new
                            {
                                src = Path.GetFileName(file),
                                labels = eval.FilteredWeights
                            };
                            Console.WriteLine("评估结果：");
                            Console.WriteLine(result.labels.ToJsonString());
                            await File.WriteAllTextAsync($@"{dir}\meta.json", result.ToJsonString());

                            stopwatch.Stop();
                            Console.WriteLine(@$"用时 [{Math.Round(stopwatch.ElapsedMilliseconds / 1000.0, 1)}s]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("评估数据集失败。", ex);
            }
        }

        static async Task AnalyzeDataset(string inputRootArg)
        {
            try
            {
                if (!Directory.Exists(inputRootArg))
                {
                    throw new InvalidOperationException("数据集目录不存在。");
                }
                var testcasesDir = Directory.GetDirectories(inputRootArg);

                Dictionary<string, double> distribution = [];
                int totalLabels = 0;

                Console.WriteLine(@$"开始分析数据集 {inputRootArg} ......");
                foreach (var dir in testcasesDir)
                {
                    var file = File.OpenRead($@"{dir}\meta.json");
                    var meta = await JsonSerializer.DeserializeAsync<TestcaseMeta>(file)!;
                    foreach (var label in meta.Labels.Keys)
                    {
                        ++totalLabels;
                        if (distribution.ContainsKey(label))
                        {
                            distribution[label] += 1;
                        }
                        else
                        {
                            distribution.Add(label, 1);
                        }
                    }
                }
                foreach (var key in distribution.Keys)
                {
                    distribution[key] /= totalLabels;
                }

                var result = distribution.Where(x => x.Value >= 0.025)
                    .OrderByDescending(x => x.Value)
                    .ToDictionary(x => x.Key, x => Math.Round(x.Value, 4));

                var rest = distribution.Where(x => x.Value < 0.025)
                    .Sum(x => x.Value);

                result.Add("others", Math.Round(rest, 4));

                Console.WriteLine(result.ToJsonString());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("评估数据集失败。", ex);
            }
        }

        static readonly JsonSerializerOptions _option = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public static string ToJsonString<T>(this T obj) where T : class
        {
            return JsonSerializer.Serialize(obj, _option);
        }
    }
}