using System;
using System.Collections.Generic;
using Amazon.CloudWatch;
using Watchman.Configuration;

namespace Watchman.Engine.Alarms
{
    public class DynamoDbDefaults
    {
        public IList<AlarmDefinition> DynamoDbGsiRead = new List<AlarmDefinition>()
        {
            new AlarmDefinition
            {
                Name = "GsiConsumedReadCapacityUnitsHigh",
                Metric = "ConsumedReadCapacityUnits",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = AwsConstants.DefaultCapacityThreshold * 100,
                    SourceAttribute = "ProvisionedReadThroughput"
                },
                DimensionNames = new[] { "GlobalSecondaryIndexName", "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "GsiReadThrottleEventsHigh",
                Metric = "ReadThrottleEvents",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.ThrottlingThreshold
                },
                DimensionNames = new[] { "GlobalSecondaryIndexName", "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            }
        };

        public IList<AlarmDefinition> DynamoDbGsiWrite = new List<AlarmDefinition>()
        {
            new AlarmDefinition
            {
                Name = "GsiConsumedWriteCapacityUnitsHigh",
                Metric = "ConsumedWriteCapacityUnits",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = AwsConstants.DefaultCapacityThreshold * 100,
                    SourceAttribute = "ProvisionedWriteThroughput"
                },
                DimensionNames = new[] { "GlobalSecondaryIndexName", "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "GsiWriteThrottleEventsHigh",
                Metric = "WriteThrottleEvents",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.ThrottlingThreshold
                },
                DimensionNames = new[] { "GlobalSecondaryIndexName", "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            }
        };


        public IList<AlarmDefinition> DynamoDbRead = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "ConsumedReadCapacityUnitsHigh",
                Metric = "ConsumedReadCapacityUnits",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = AwsConstants.DefaultCapacityThreshold * 100,
                    SourceAttribute = "ProvisionedReadThroughput"
                },
                DimensionNames = new[] { "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "ReadThrottleEventsHigh",
                Metric = "ReadThrottleEvents",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.ThrottlingThreshold
                },
                DimensionNames = new[] { "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            }
        };


        public IList<AlarmDefinition> DynamoDbWrite = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "ConsumedWriteCapacityUnitsHigh",
                Metric = "ConsumedWriteCapacityUnits",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = AwsConstants.DefaultCapacityThreshold * 100,
                    SourceAttribute = "ProvisionedWriteThroughput"
                },
                DimensionNames = new[] { "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "WriteThrottleEventsHigh",
                Metric = "WriteThrottleEvents",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.ThrottlingThreshold
                },
                DimensionNames = new[] { "TableName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.DynamoDb,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            }
        };
    }

    /// <summary>
    ///  Alarms which will be applied to each service
    ///  The threshold can be overridden by the service or resource definition
    /// </summary>
    public static class Defaults
    {
        public static IList<AlarmDefinition> Rds = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "FreeStorageSpaceLow",
                Metric = "FreeStorageSpace",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    SourceAttribute = "AllocatedStorage",
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 30
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.LessThanThreshold,
                Statistic = Statistic.Minimum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "CPUUtilizationHigh",
                Metric = "CPUUtilization",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 5,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 60
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "DatabaseConnectionsHigh",
                Metric = "DatabaseConnections",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 200
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "ReadLatencyHigh",
                Metric = "ReadLatency",
                Enabled = false,
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 300
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "WriteLatencyHigh",
                Metric = "WriteLatency",
                Enabled = false,
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 300
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "ReadIOPSHigh",
                Metric = "ReadIOPS",
                Enabled = false,
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    SourceAttribute = "Iops",
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 80
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            },
            new AlarmDefinition
            {
                Name = "WriteIOPSHigh",
                Metric = "WriteIOPS",
                Enabled = false,
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    SourceAttribute = "Iops",
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 80
                },
                DimensionNames = new[] { "DBInstanceIdentifier" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Rds
            }
        };

        public static IList<AlarmDefinition> AutoScaling = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "CPUCreditBalanceLow",
                Metric = "CPUCreditBalance",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 0.2
                },
                DimensionNames = new[] { "AutoScalingGroupName" },
                ComparisonOperator = ComparisonOperator.LessThanThreshold,
                Statistic = Statistic.Minimum,
                Namespace = AwsNamespace.Ec2
            },
            new AlarmDefinition
            {
                Name = "CPUUtilizationHigh",
                Metric = "CPUUtilization",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 90
                },
                DimensionNames = new[] { "AutoScalingGroupName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Ec2
            },
            new AlarmDefinition
            {
                Name = "GroupInServiceInstancesLow",
                Metric = "GroupInServiceInstances",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 50,
                    SourceAttribute = "GroupDesiredCapacity"
                },
                DimensionNames = new[] { "AutoScalingGroupName" },
                ComparisonOperator = ComparisonOperator.LessThanThreshold,
                Statistic = Statistic.Minimum,
                Namespace = AwsNamespace.AutoScaling
            }
        };

        public static IList<AlarmDefinition> Lambda = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "ErrorsHigh",
                Metric = "Errors",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 3
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Lambda,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "DurationHigh",
                Metric = "Duration",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 50,
                    SourceAttribute = "Timeout"
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Lambda
            },
            new AlarmDefinition
            {
                Name = "ThrottlesHigh",
                Metric = "Throttles",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 5
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Lambda,
                TreatMissingData = TreatMissingDataConstants.NotBreaching
            },
            new AlarmDefinition
            {
                Name = "IteratorAgeHigh",
                Metric = "IteratorAge",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 300000
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Lambda
            },
            new AlarmDefinition
            {
                Name = "InvocationsLow",
                Enabled = false,
                Metric = "Invocations",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 288,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.LessThanThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Lambda,
                TreatMissingData = TreatMissingDataConstants.Breaching
            },
            new AlarmDefinition
            {
                Name = "InvocationsHigh",
                Enabled = false,
                Metric = "Invocations",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 288,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "FunctionName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.SampleCount, //The count (number) of data points used for the statistical calculation. Is this really what we want?
                Namespace = AwsNamespace.Lambda
            }
        };

        public static IList<AlarmDefinition> VpcSubnets = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "IpAddressesRemainingLow",
                Metric = "VPC_AvailableIpAddresses",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 4,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    SourceAttribute = "NumberOfIpAddresses",
                    Value = 30
                },
                DimensionNames = new[] { "Subnet" },
                ComparisonOperator = ComparisonOperator.LessThanOrEqualToThreshold,
                Statistic = Statistic.Minimum,
                Namespace = "JUSTEAT/PlatformLimits",
                AlertOnInsufficientData = true
            }
        };

        public static IList<AlarmDefinition> Elb = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "Elb5xxErrorsHigh",
                Metric = "HTTPCode_ELB_5XX",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 50
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Elb
            },
            new AlarmDefinition
            {
                Name = "Http5xxErrorsHigh",
                Metric = "HTTPCode_Backend_5XX",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 500
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Elb
            },
            new AlarmDefinition
            {
                Name = "SurgeQueueLengthHigh",
                Metric = "SurgeQueueLength",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 200
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Elb
            },
            new AlarmDefinition
            {
                Name = "SpilloverCountHigh",
                Metric = "SpilloverCount",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 10
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Elb
            },
            new AlarmDefinition
            {
                Name = "LatencyHigh",
                Metric = "Latency",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 0.50
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                // This should be probably a percentile statistic once that is supported via cloudformation
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Elb
            },
            new AlarmDefinition
            {
                Name = "UnHealthyHostCountHigh",
                Metric = "UnHealthyHostCount",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 4,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "LoadBalancerName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Elb
            }
        };

        public static IList<AlarmDefinition> Alb = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "5xxErrorsHigh",
                Metric = "HTTPCode_ELB_5XX_Count",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 10
                },
                DimensionNames = new[] {"LoadBalancer"},
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Alb
            },
            new AlarmDefinition
            {
                Name = "Target5xxErrorsHigh",
                Metric = "HTTPCode_Target_5XX_Count",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 10
                },
                DimensionNames = new[] {"LoadBalancer"},
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Alb
            },
            new AlarmDefinition
            {
                Name = "RejectedConnectionCountHigh",
                Metric = "RejectedConnectionCount",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 10
                },
                DimensionNames = new[] {"LoadBalancer"},
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Alb
            },
            new AlarmDefinition
            {
                Name = "TargetResponseTimeHigh",
                Metric = "TargetResponseTime",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 2,
                ExtendedStatistic = "p99",
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 2
                },
                DimensionNames = new[] {"LoadBalancer"},
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Namespace = AwsNamespace.Alb
            }
        };

        public static IList<AlarmDefinition> KinesisStream = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "ReadProvisionedThroughputExceededHigh",
                Metric = "ReadProvisionedThroughputExceeded",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "StreamName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Kinesis
            },
            new AlarmDefinition
            {
                Name = "WriteProvisionedThroughputExceededHigh",
                Metric = "WriteProvisionedThroughputExceeded",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "StreamName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.Kinesis
            }
        };


        public static IList<AlarmDefinition> StepFunction = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "ExecutionsFailedHigh",
                Metric = "ExecutionsFailed",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "StateMachineArn" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Sum,
                Namespace = AwsNamespace.StepFunction
            }
        };

        public static IList<AlarmDefinition> Sqs = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "NumberOfVisibleMessages",
                Metric = "ApproximateNumberOfMessagesVisible",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.QueueLengthThreshold
                },
                DimensionNames = new[] { "QueueName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Sqs
            },
            new AlarmDefinition
            {
                Name = "AgeOfOldestMessage",
                Metric = "ApproximateAgeOfOldestMessage",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.OldestMessageThreshold
                },
                DimensionNames = new[] { "QueueName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Sqs
            }
        };

        public static IList<AlarmDefinition> SqsError = new List<AlarmDefinition>
        {
            new AlarmDefinition
            {
                Name = "NumberOfVisibleMessages_Error",
                Metric = "ApproximateNumberOfMessagesVisible",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.ErrorQueueLengthThreshold
                },
                DimensionNames = new[] { "QueueName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.Sqs
            },
            new AlarmDefinition
            {
                Name = "AgeOfOldestMessage_Error",
                Metric = "ApproximateAgeOfOldestMessage",
                Period = TimeSpan.FromMinutes(5),
                EvaluationPeriods = 1,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = AwsConstants.OldestMessageThreshold
                },
                DimensionNames = new[] { "QueueName" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Maximum,
                Namespace = AwsNamespace.Sqs
            }
        };

        public static IList<AlarmDefinition> ElastiCache = new List<AlarmDefinition>
        { 
            new AlarmDefinition
            {
                Enabled = true,
                Name = "CPUUtilizationHigh",
                Metric = "CPUUtilization",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 5,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.PercentageOf,
                    Value = 60
                },
                DimensionNames = new[] { "CacheNodeId" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.Average,
                Namespace = AwsNamespace.ElastiCache,
            },
            new AlarmDefinition
            {
                Enabled = true,
                Name = "EvictionStarted",
                Metric = "Evictions",
                Period = TimeSpan.FromMinutes(1),
                EvaluationPeriods = 5,
                Threshold = new Threshold
                {
                    ThresholdType = ThresholdType.Absolute,
                    Value = 1
                },
                DimensionNames = new[] { "CacheNodeId" },
                ComparisonOperator = ComparisonOperator.GreaterThanOrEqualToThreshold,
                Statistic = Statistic.SampleCount,
                Namespace = AwsNamespace.ElastiCache,
            }
        };
    }
}
