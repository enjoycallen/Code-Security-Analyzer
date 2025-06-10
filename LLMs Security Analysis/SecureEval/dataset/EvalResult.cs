namespace SecureEval
{
    internal class EvalResult
    {
        public Dictionary<string, double> Weights { get; set; } = [];

        public Dictionary<string, double> FilteredWeights => Weights.Where(x => x.Value > 0.3)
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToDictionary(kv => kv.Key, kv => Math.Round(kv.Value, 3));

        public void Add(string label, double weight)
        {
            Weights.TryAdd(label, weight);
        }

        public void Update(EvalResult other)
        {
            foreach (var (key, value) in other.Weights)
            {
                if (!Weights.TryAdd(key, value))
                {
                    Weights[key] += value;
                }
            }
        }
    }
}
