using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CloudWatch.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Watchman.AwsResources;
using Watchman.AwsResources.Services.DynamoDb;
using Watchman.Configuration;
using Watchman.Configuration.Generic;
using Watchman.Engine.Alarms;

namespace Watchman.Engine.Generation.Dynamo
{
    public class DynamoResourceAlarmGenerator : IResourceAlarmGenerator<TableDescription, ResourceConfig>
    {
        private readonly IResourceSource<TableDescription> _tableSource;
        private readonly IAlarmDimensionProvider<TableDescription> _dimensions;
        private readonly IResourceAttributesProvider<TableDescription, ResourceConfig> _attributeProvider;
        private readonly DynamoDbGsiDataProvider _gsiProvider = new DynamoDbGsiDataProvider();
        private readonly AlarmDefaults<TableDescription> _defaultAlarms;


        public DynamoResourceAlarmGenerator(
            IResourceSource<TableDescription> tableSource,
            IAlarmDimensionProvider<TableDescription> dimensionProvider,
            IResourceAttributesProvider<TableDescription, ResourceConfig> attributeProvider, AlarmDefaults<TableDescription> defaultAlarms)
        {
            _tableSource = tableSource;
            _dimensions = dimensionProvider;
            _attributeProvider = attributeProvider;
            _defaultAlarms = defaultAlarms;
        }

        public async Task<IList<Alarm>>  GenerateAlarmsFor(
            AwsServiceAlarms<ResourceConfig> service,
            AlertingGroupParameters groupParameters)
        {
            if (service?.Resources == null || service.Resources.Count == 0)
            {
                return new List<Alarm>();
            }

            List<Alarm> alarms = new List<Alarm>();

            foreach (var resource in service.Resources)
            {
                var alarmsForResource = await CreateAlarmsForResource(resource, service, groupParameters);
                alarms.AddRange(alarmsForResource);
            }

            return alarms;
        }

        private async Task<IList<Alarm>> CreateAlarmsForResource(
            ResourceThresholds<ResourceConfig> resource,
            AwsServiceAlarms<ResourceConfig> service,
            AlertingGroupParameters groupParameters)
        {
            var entity = await _tableSource.GetResourceAsync(resource.Name);

            if (entity == null)
            {
                throw new Exception($"Entity {resource.Name} not found");
            }

            var result = await BuildTableAlarms(resource, service, groupParameters, entity);

            result.AddRange(await BuildIndexAlarms(resource, service, groupParameters, entity.Resource));

            return result;
        }

        private async Task<List<Alarm>> BuildTableAlarms(ResourceThresholds<ResourceConfig> resourceConfig,
            AwsServiceAlarms<ResourceConfig> service,
            AlertingGroupParameters groupParameters,
            AwsResource<TableDescription> entity)
        {
            var mergedConfig = service.Options.OverrideWith(resourceConfig.Options);

            var result = new List<Alarm>();

            var mergedValuesByAlarmName = service.Values.OverrideWith(resourceConfig.Values);

            foreach (var alarm in _defaultAlarms)
            {
                var dimensions = _dimensions.GetDimensions(entity.Resource, alarm.DimensionNames);
                var values = mergedValuesByAlarmName.GetValueOrDefault(alarm.Name) ?? new AlarmValues();
                var configuredThreshold = alarm.Threshold.CopyWith(value: values.Threshold);

                var threshold = await ThresholdCalculator.ExpandThreshold(_attributeProvider,
                    entity.Resource,
                    mergedConfig,
                    configuredThreshold);

                var built = alarm.CopyWith(threshold, values);

                var model = new Alarm
                {
                    AlarmName = $"{resourceConfig.Name}-{built.Name}-{groupParameters.AlarmNameSuffix}",
                    AlarmDescription = groupParameters.DefaultAlarmDescription(),
                    Resource = entity,
                    Dimensions = dimensions,
                    AlarmDefinition = built
                };

                result.Add(model);
            }

            return result;
        }

        private async Task<IList<Alarm>> BuildIndexAlarms(ResourceThresholds<ResourceConfig> resource,
            AwsServiceAlarms<ResourceConfig> service,
            AlertingGroupParameters groupParameters,
            TableDescription table)
        {
            // called twice
            var mergedConfig = service.Options.OverrideWith(resource.Options);

            var result = new List<Alarm>();

            var gsiSet = table.GlobalSecondaryIndexes;

            var mergedValuesByAlarmName = service.Values.OverrideWith(resource.Values);

            foreach (var gsi in gsiSet)
            {
                var gsiResource = new AwsResource<GlobalSecondaryIndexDescription>(gsi.IndexName, gsi);

                foreach (var alarm in Defaults.DynamoDbGsi)
                {
                    var values = mergedValuesByAlarmName.GetValueOrDefault(alarm.Name) ?? new AlarmValues();
                    var configuredThreshold = alarm.Threshold.CopyWith(value: values.Threshold);
                    var dimensions = _gsiProvider.GetDimensions(gsi, alarm.DimensionNames);
                    var threshold = await ThresholdCalculator.ExpandThreshold(_gsiProvider,
                        gsiResource.Resource,
                        mergedConfig,
                        configuredThreshold);

                    var built = alarm.CopyWith(threshold, values);

                    var model = new Alarm
                    {
                        AlarmName = $"{resource.Name}-{gsi.IndexName}-{alarm.Name}-{groupParameters.AlarmNameSuffix}",
                        AlarmDescription = groupParameters.DefaultAlarmDescription(),
                        Resource = gsiResource,
                        Dimensions = dimensions,
                        AlarmDefinition = built
                    };

                    result.Add(model);
                }
            }

            return result;
        }
    }
}
