using System.Runtime.CompilerServices;

namespace SecureEval
{
    internal class Graph
    {
        class Node(int id, string label)
        {
            public int ID { get; init; } = id;

            public string Label { get; init; } = label;
        }
        class Nodes : List<Node>;


        readonly Nodes _nodes = [];
        readonly Dictionary<string, int> _nodeIdFromLabel = [];
        public void AddNode(string label)
        {
            if (_nodeIdFromLabel.ContainsKey(label))
            {
                throw new InvalidOperationException("节点编号重复");
            }

            Node node = new(_nodes.Count, label);
            _nodes.Add(node);
            _nodeIdFromLabel.Add(node.Label, node.ID);
            _parents.Add([]);
            _childs.Add([]);
        }

        int GetIdByLabel(string label)
        {
            return _nodeIdFromLabel.TryGetValue(label, out var id) ? id
                : throw new KeyNotFoundException($"不存在标签为 {label} 的节点");
        }


        readonly List<List<int>> _parents = [];
        readonly List<List<int>> _childs = [];
        public void AddEdge(string u_label, string v_label)
        {
            var u = GetIdByLabel(u_label);
            var v = GetIdByLabel(v_label);
            if (!_parents[v].Contains(u))
            {
                _childs[u].Add(v);
                _parents[v].Add(u);
            }
        }


        bool[,] _reachable;
        public void FloydWarshall()
        {
            var n = _nodes.Count;
            _reachable = new bool[n, n];
            for (int id = 0; id < n; ++id)
            {
                _reachable[id,id] = true;
                foreach (var child in _childs[id])
                {
                    _reachable[id,child] = true;
                }
            }

            for (int k = 0; k < n; ++k)
            {
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        _reachable[i, j] = _reachable[i, j] || (_reachable[i, k] && _reachable[k, j]);
                    }
                }
            }
        }

        public MatchResult Match(string u_label, string v_label)
        {
            var u = GetIdByLabel(u_label);
            var v = GetIdByLabel(v_label);
            
            MatchResult res = new();
            for (int id = 0; id < _nodes.Count; ++id)
            {
                if (_reachable[u, id])
                {
                    if (_reachable[v, id]) ++res.TP;
                    else ++res.FN;
                }
                else
                {
                    if (_reachable[v, id]) ++res.FP;
                    else ++res.TN;
                }
            }

            return res;
        }
    }
}