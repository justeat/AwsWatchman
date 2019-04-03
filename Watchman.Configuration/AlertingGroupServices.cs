using System.Collections.Generic;
using System.Linq;
using Watchman.Configuration.Generic;

namespace Watchman.Configuration
{
    public class AlertingGroupServices
    {
        public AwsServiceAlarms<ResourceConfig> Rds { get; set;  }
        public AwsServiceAlarms<AutoScalingResourceConfig> AutoScaling { get; set; }
        public AwsServiceAlarms<ResourceConfig> Lambda { get; set; }
        public AwsServiceAlarms<ResourceConfig> VpcSubnet { get; set; }
        public AwsServiceAlarms<ResourceConfig> Elb { get; set; }
        public AwsServiceAlarms<ResourceConfig> Alb { get; set; }
        public AwsServiceAlarms<ResourceConfig> KinesisStream { get; set; }
        public AwsServiceAlarms<ResourceConfig> StepFunction { get; set; }
        public AwsServiceAlarms<DynamoResourceConfig> DynamoDb { get; set; }
        public AwsServiceAlarms<SqsResourceConfig> Sqs { get; set; }
        public AwsServiceAlarms<ResourceConfig> ElastiCache { get; set; } //todo MT is this ok?


        public IList<IAwsServiceAlarms> AllServices => new IAwsServiceAlarms[]
            { Rds, AutoScaling, Lambda, VpcSubnet, Elb, KinesisStream, StepFunction, DynamoDb, Sqs }
            .Where(s => s != null)
            .ToArray();

        public Dictionary<string, IAwsServiceAlarms> AllServicesByName => new Dictionary<string, IAwsServiceAlarms>()
        {
            {"Rds", Rds},
            {"AutoScaling", AutoScaling},
            {"Lambda", Lambda},
            {"VpcSubnet", VpcSubnet},
            {"Elb", Elb},
            {"Alb", Alb},
            {"KinesisStream", KinesisStream},
            {"StepFunction", StepFunction},
            {"DynamoDb", DynamoDb},
            {"Sqs", Sqs },
            {"ElastiCache", ElastiCache }
        };
    }
}
