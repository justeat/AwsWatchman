using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Watchman.AwsResources;
using Watchman.Configuration;
using Watchman.Configuration.Generic;

namespace Watchman.Engine.Generation
{
    static class AlarmHelpers
    {
        public static string GetAlarmDescription(AlertingGroupParameters groupParameters)
        {
            var suffix = string.IsNullOrWhiteSpace(groupParameters.Description)
                ? null
                : $" ({groupParameters.Description})";

            var description = $"{AwsConstants.DefaultDescription}. Alerting group: {groupParameters.Name}{suffix}";

            return description;
        }

        public static async Task<AlarmDefinition> AlarmWithMergedValues<T, TAlarmConfig>(
            IResourceAttributesProvider<T, TAlarmConfig> attributeProvider,
            AwsResource<T> entity,
            AlarmDefinition alarm,
            TAlarmConfig mergedConfig,
            AlarmValues mergedValues) where T : class
            where TAlarmConfig : class, IServiceAlarmConfig<TAlarmConfig>, new()
        {
            var copy = alarm.Copy();

            copy.Threshold = await ExpandThreshold(attributeProvider, entity.Resource, mergedConfig, new Threshold
            {
                SourceAttribute = alarm.Threshold.SourceAttribute,
                ThresholdType = alarm.Threshold.ThresholdType,
                Value = mergedValues.Threshold ?? alarm.Threshold.Value
            });

            copy.EvaluationPeriods = mergedValues.EvaluationPeriods ?? alarm.EvaluationPeriods;

            copy.ExtendedStatistic = !string.IsNullOrEmpty(mergedValues.ExtendedStatistic)
                ? mergedValues.ExtendedStatistic
                : alarm.ExtendedStatistic;

            copy.Enabled = mergedValues.Enabled ?? alarm.Enabled;

            return copy;
        }

        private static async Task<Threshold> ExpandThreshold<T, TAlarmConfig>(
            IResourceAttributesProvider<T, TAlarmConfig> attributeProvider,
            T resource, TAlarmConfig config, Threshold threshold)
            where TAlarmConfig : class, IServiceAlarmConfig<TAlarmConfig>, new()
        {
            if (threshold.ThresholdType == ThresholdType.PercentageOf)
            {
                var fraction = threshold.Value / 100;
                var property = await attributeProvider.GetValue(resource, config, threshold.SourceAttribute);

                threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = fraction * (double) property
                };
            }

            return threshold;
        }
    }
}
