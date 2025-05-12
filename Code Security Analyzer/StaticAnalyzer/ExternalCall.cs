using System.Diagnostics;
using System.Text;

namespace StaticAnalyzer
{
    internal class ExternalCall : IDisposable
    {
        readonly Process _process;

        public StreamWriter? Input => _process.StartInfo.RedirectStandardInput ? _process.StandardInput : null;

        public StreamReader? OutputStream => _process.StartInfo.RedirectStandardOutput ? _process.StandardOutput : null;

        public StreamReader? ErrorStream => _process.StartInfo.RedirectStandardError ? _process.StandardError : null;

        public int ExitedCode => _process.ExitCode;

        public ExternalCall(string path, string? args = null, RedirectOption option = RedirectOption.None)
        {
            //Console.WriteLine(path + " " + args);
            ProcessStartInfo psi = new()
            {
                FileName = path,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = option.HasFlag(RedirectOption.StandardInput),
                RedirectStandardOutput = option.HasFlag(RedirectOption.StandardOutput),
                RedirectStandardError = option.HasFlag(RedirectOption.StandardError)
            };

            _process = new() { StartInfo = psi };
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