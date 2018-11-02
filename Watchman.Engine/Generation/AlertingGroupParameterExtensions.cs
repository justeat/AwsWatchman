using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Watchman.AwsResources;
using Watchman.Configuration;
using Watchman.Configuration.Generic;

namespace Watchman.Engine.Generation
{
    static class AlertingGroupParameterExtensions
    {
        public static string DefaultAlarmDescription(this AlertingGroupParameters groupParameters)
        {
            var suffix = string.IsNullOrWhiteSpace(groupParameters.Description)
                ? null
                : $" ({groupParameters.Description})";

            var description = $"{AwsConstants.DefaultDescription}. Alerting group: {groupParameters.Name}{suffix}";

            return description;
        }
    }
}
