using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SecureEval
{
    internal static class CweRelations
    {
        static readonly CweMeta _meta;
        static readonly Dictionary<string, CweMeta.Weakness> _weaknessesFromId = [];
        static readonly Graph _graph = new();

        static CweRelations()
        {
            using var file = File.OpenRead("meta.json");
            _meta = JsonSerializer.Deserialize<CweMeta>(file)
                ?? throw new InvalidOperationException("无法加载 CWE 元数据");

            foreach (var weakness in _meta.Weaknesses)
            {
                _weaknessesFromId.Add(weakness.ID, weakness);
                _graph.AddNode(weakness.ID);
                weakness.Parents.ForEach(parentId => _graph.AddEdge(parentId, weakness.ID));
            }
            _graph.FloydWarshall();
        }

        static CweMeta.Weakness GetWeaknessById(string id)
        {
            return _weaknessesFromId.TryGetValue(id, out var weakness) ? weakness
                : throw new KeyNotFoundException($"编号 {id} 不存在");
        }

        public static MatchResult Match(string id1, string id2)
        {
            return _graph.Match(id1, id2);
        }
    }
}