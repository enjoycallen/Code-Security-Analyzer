namespace SecureEval
{
    internal class MatchResult
    {
        public int TP = 0;
        public int FP = 0;
        public int TN = 0;
        public int FN = 0;

        public double Precision => TP + FP == 0 ? 0 : (double)TP / (TP + FP);

        public double Recall => TP + FN == 0 ? 0 : (double)TP / (TP + FN);

        public double F1Score => Precision + Recall == 0 ? 0 : 2 * Precision * Recall / (Precision + Recall);

        public double FPR => TN + FP == 0 ? 0 : (double)FP / (TN + FP);

        public double TPR => Recall;
    }
}