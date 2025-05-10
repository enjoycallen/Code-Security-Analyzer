using System.Diagnostics;

namespace StaticAnalyzer
{
    internal class ExternalCall : IDisposable
    {
        readonly Process _process;

        public Stream? InputStream => _process.StartInfo.RedirectStandardInput ? _process.StandardInput.BaseStream : null;

        public Stream? OutputStream => _process.StartInfo.RedirectStandardOutput ? _process.StandardOutput.BaseStream : null;

        public Stream? ErrorStream => _process.StartInfo.RedirectStandardError ? _process.StandardError.BaseStream : null;

        public int ExitedCode => _process.ExitCode;

        public ExternalCall(string path, string args, RedirectOption option)
        {
            //Console.WriteLine(path + " " + args);
            _process = new()
            {
                StartInfo = new()
                {
                    FileName = path,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = option.HasFlag(RedirectOption.StandardInput),
                    RedirectStandardOutput = option.HasFlag(RedirectOption.StandardOutput),
                    RedirectStandardError = option.HasFlag(RedirectOption.StandardError)
                }
            };
            _process.Start();
        }

        public void WaitForExit() => _process.WaitForExit();

        public async Task WaitForExitAsync() => await _process.WaitForExitAsync();

        public void Terminate()
        {
            if (!_process.HasExited)
            {
                _process.Kill();
            }
        }

        public void Dispose()
        {
            Terminate();
            _process.Dispose();
        }
    }

    [Flags]
    public enum RedirectOption
    {
        None = 0,
        StandardInput = 1,
        StandardOutput = 2,
        StandardError = 4,
    }
}