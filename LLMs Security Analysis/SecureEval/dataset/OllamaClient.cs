using System.Net.Http.Json;
using System.Text.Json;

namespace SecureEval
{
    internal static class OllamaClient
    {
        static HttpClient? _client;

        public static void Init()
        {
            if (_client is not null) return;
            _client = new()
            {
                BaseAddress = new("http://localhost:11434"),
                Timeout = TimeSpan.FromMinutes(3)
            };
        }

        public static void Close()
        {
            _client?.Dispose();
        }

        public static async Task<string> RequestAsync(string modelName, string prompt)
        {
            if (_client is null)
            {
                throw new InvalidOperationException("OllamaClient 未初始化。请先调用 OllamaClient.Init()。");
            }

            try
            {
                using var response = await _client.PostAsJsonAsync("/api/generate", new
                {
                    model = modelName,
                    prompt,
                    stream = false
                });
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("response").GetString() ?? "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}