using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Watchman.Configuration;

namespace Watchman.Engine.Generation.Generic
{
    public class CloudWatchCloudFormationTemplate
    {
        private readonly string _groupName;
        private readonly IList<AlertTarget> _targets;
        private static readonly Regex NonAlpha = new Regex("[^a-zA-Z0-9]+");

        private readonly List<Alarm> _alarms = new List<Alarm>();

        private string _emailTopicResourceName;
        private string _urlTopicResourceName;

        public CloudWatchCloudFormationTemplate(string groupName, IList<AlertTarget> targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            _groupName = groupName;
            _targets = targets;
        }

        public void AddAlarms(IEnumerable<Alarm> alarms)
        {
            _alarms.AddRange(alarms);
        }
        public void AddAlarm(Alarm alarm)
        {
            _alarms.Add(alarm);
        }

        public string WriteJson()
        {
            var root = new JObject();
            root["AWSTemplateFormatVersion"] = "2010-09-09";
            root["Description"] = $"Stack generated by AwsWatchman. Contains {_alarms.Count} Cloudwatch alarms";

            var resources = new JObject();

            AddSnsTopics(_groupName, resources);

            foreach (var alarm in _alarms)
            {
                var resourceName = NonAlpha.Replace(alarm.Resource.Name + alarm.AlarmDefinition.Name, "");
                var alarmJson = BuildAlarmJson(alarm);

                resources[resourceName] = alarmJson;
            }

            root["Resources"] = resources;
            return root.ToString();
        }

        private static JObject CreateSnsTopic<T>(string description, List<T> targets, Func<T, JObject> mapper) where T : AlertTarget
        {
            var sns = JObject.FromObject(new
            {
                Type = "AWS::SNS::Topic",
                Properties = new
                {
                    DisplayName = description
                }
            });

            var subscriptions = targets.Select(mapper);
            sns["Properties"]["Subscription"] = new JArray(subscriptions);
            return sns;
        }

        private void AddSnsTopics(string groupName, JObject resources)
        {
            var emails = _targets.OfType<AlertEmail>().ToList();

            if (emails.Any())
            {
                var sns = CreateSnsTopic(AwsConstants.DefaultEmailTopicDesciption,
                    emails, email => JObject.FromObject(new
                        {
                            Protocol = "email",
                            Endpoint = email.Email
                        }));

                _emailTopicResourceName = "EmailTopic";

                resources[_emailTopicResourceName] = sns;
            }

            var urls = _targets.OfType<AlertUrl>().ToList();
            if (urls.Any())
            {
                var sns = CreateSnsTopic(AwsConstants.DefaultUrlTopicDesciption,
                    urls, url =>
                {
                    var protocol = url.Url.StartsWith("https") ? "https" : "http";
                    return JObject.FromObject(new
                    {
                        Protocol = protocol,
                        Endpoint = url.Url
                    });
                });

                _urlTopicResourceName = "UrlTopic";
                resources[_urlTopicResourceName] = sns;
            }
        }

        private JObject BuildAlarmJson(Alarm alarm)
        {
            var alarmJson = new JObject();
            alarmJson["Type"] = "AWS::CloudWatch::Alarm";
            alarmJson["Properties"] = BuildAlarmPropertiesJson(alarm);
            return alarmJson;
        }

        private JObject BuildAlarmPropertiesJson(Alarm alarm)
        {
            var definition = alarm.AlarmDefinition;

            var propsObject = new
            {
                AlarmName = alarm.AlarmName,
                AlarmDescription = alarm.AlarmDescription,
                Namespace = definition.Namespace,
                MetricName = definition.Metric,
                Dimensions = alarm.Dimensions.Select(d => new { d.Name, d.Value }),
                ComparisonOperator = definition.ComparisonOperator.Value,
                EvaluationPeriods = definition.EvaluationPeriods,
                Period = (int) definition.Period.TotalSeconds,
                Threshold = definition.Threshold.Value,
                TreatMissingData = definition.TreatMissingData
            };

            var result = JObject.FromObject(propsObject);

            if (string.IsNullOrEmpty(definition.ExtendedStatistic))
            {
                result["Statistic"] = definition.Statistic.Value;
            }
            else
            {
                result["ExtendedStatistic"] = definition.ExtendedStatistic;
            }

            if (definition.AlertOnInsufficientData)
            {
                result["InsufficientDataActions"] = TargetRefs(email: true, url: true);
            }

            if (definition.AlertOnOk)
            {
                result["OKActions"] = TargetRefs(email: false, url: true);
            }

            result["AlarmActions"] = TargetRefs(email: true, url: true);

            return result;
        }

        private JArray TargetRefs(bool email, bool url)
        {
            return new JArray(TargetResourceNames(email, url).Select(t => JObject.FromObject(new
            {
                Ref = t
            })));
        }

        private IEnumerable<string> TargetResourceNames(bool email, bool url)
        {
            if (email && !string.IsNullOrEmpty(_emailTopicResourceName))
            {
                yield return _emailTopicResourceName;
            }

            if (url && !string.IsNullOrEmpty(_urlTopicResourceName))
            {
                yield return _urlTopicResourceName;
            }
        }
    }
}
