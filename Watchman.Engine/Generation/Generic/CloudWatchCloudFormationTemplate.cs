using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Watchman.Engine.Generation.Generic
{
    static class CloudWatchCloudFormationTemplate
    {
        private static readonly Regex NonAlpha = new Regex("[^a-zA-Z0-9]+");

        public static string WriteJson(IEnumerable<Alarm> alarms)
        {
            var root = new JObject();
            root["AWSTemplateFormatVersion"] = "2010-09-09";

            var resources = new JObject();
            root["Resources"] = resources;

            foreach (var alarm in alarms)
            {
                var resourceName = NonAlpha.Replace(alarm.Resource.Name + alarm.AlarmDefinition.Name, "");
                var alarmJson = BuildAlarmJson(alarm);

                resources[resourceName] = alarmJson;
            }

            return root.ToString();
        }

        private static JObject BuildAlarmJson(Alarm alarm)
        {
            var alarmJson = new JObject();
            alarmJson["Type"] = "AWS::CloudWatch::Alarm";
            alarmJson["Properties"] = BuildAlarmPropertiesJson(alarm);
            return alarmJson;
        }

        private static JObject BuildAlarmPropertiesJson(Alarm alarm)
        {
            var definition = alarm.AlarmDefinition;
            var insufficientDataActions = ValueOrEmpty(definition.AlertOnInsufficientData, alarm.SnsTopicArn);
            var okActions = ValueOrEmpty(definition.AlertOnOk, alarm.SnsTopicArn);

            var propsObject = new
            {
                AlarmName = alarm.AlarmName,
                AlarmDescription = AwsConstants.DefaultDescription,
                Namespace = definition.Namespace,
                MetricName = definition.Metric,
                Dimensions = alarm.Dimensions.Select(d => new { d.Name, d.Value }),
                AlarmActions = new[] {alarm.SnsTopicArn},
                OKActions = okActions,
                InsufficientDataActions = insufficientDataActions,
                ComparisonOperator = definition.ComparisonOperator.Value,
                EvaluationPeriods = definition.EvaluationPeriods,
                Period = (int) definition.Period.TotalSeconds,
                Statistic = definition.Statistic.Value,
                Threshold = definition.Threshold.Value
            };

            return JObject.FromObject(propsObject);
        }

        private static IEnumerable<string> ValueOrEmpty(bool hasValue, string value)
        {
            return hasValue ? new[] {value} : Enumerable.Empty<string>();
        }
    }
}
