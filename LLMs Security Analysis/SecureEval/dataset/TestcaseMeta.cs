using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecureEval
{
    internal class TestcaseMeta
    {
        [JsonPropertyName("src")]
        public string SourceFile { get; set; }

        [JsonPropertyName("labels")]
        public Dictionary<string,double> Labels { get; set; }
    }
}
