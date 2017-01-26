namespace Watchman.Configuration.Generic
{
    public class ThresholdValue
    {
        public double Value { get; }
        public int EvaluationPeriods { get; }

        public ThresholdValue()
        {
            Value = 0;
            EvaluationPeriods = 1;
        }

        public ThresholdValue(double value, int evaluationPeriods)
        {
            Value = value;
            EvaluationPeriods = evaluationPeriods;
        }

        public static implicit operator ThresholdValue(double value)
        {
            return new ThresholdValue(value, 1);
        }
    }
}