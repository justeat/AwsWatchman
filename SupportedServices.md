# Configuration of other service types

## Alerting group

One or more alerting groups should be defined in the config folder that you supply to watchman.

```
{
  "Name": "TEST",
  "AlarmNameSuffix": "TestingGroup",
  "Targets": [
    { "Email": "email@just-eat.com" }
  ],
  "Services": {
  	"ServiceIdentifier": { /* service */ }
  }
}

```

## Service definition

Configuration for the additional services is added to the alerting group under the "Services" property. (note that configuration for Dynamo and SQS requires a different configuration format - see existing documentation).


They all require an object in this format:

```
{
	// [optional] defines default thresholds and evaluation periods for the alerting group
	"Values": {
  		"AlarmName": 31
  	},

    // [optional] specific options for this service type
    "Options": {
        "InstanceCountIncreaseDelayMinutes": 10
    },

  	// Resources for this service type
	"Resources": [
		// regular expression match of resource name
		{
       		"Pattern": "^.*$", 

       		// [optional] override the thresholds defined elsewhere
        	"Values": {
        		"AlarmName": 11
        	},

            // [optional] override specific options for this service type
            "Options": {
                "InstanceCountIncreaseDelayMinutes": 5
            },
     	},

     	// exact match of resource name
     	{
       		"Name": "Resource"        	
     	},

     	// or
     	"ResourceName"
     ]	
}
```

# Overriding threshold and other default attributes

As in the above example, the threshold can be overriden by including the following (at either resource or service level):

```
  "Values": {
     "AlarmName": 11
  }
```

An alarm can be disabled using:

```
  "Values": {
     "AlarmName": false
  }
```

Multiple attributes can be overridden if an object is specified:
- `Threshold`: threshold (will be either an absolute value or a percentage - see below)
- `EvaluationPeriods`: number of periods for which the threshold must be breached, in order to trigger the alarm
- `ExtendedStatistic`: instead of the default statistic (e.g. average, max, etc.) use a percentile e.g. "p99"). See [AWS documentation](https://docs.aws.amazon.com/AmazonCloudWatch/latest/monitoring/cloudwatch_concepts.html#Percentiles.
- `Enabled`: Whether the alarm is disabled or not. Currently all alarms are default enabled - this might change in future for certain types.

For example:

```
  "Values": {
     "AlarmName": {
        "Threshold": 11,
        "EvaluationPeriods": 2,
        "ExtendedStatistic": "p99.9"
      }
  }
```

Note that only the values you want to override need to be defined.

# Resource types

The following services are supported

- `Rds`
- `AutoScaling`
- `Lambda`
- `Kinesis`
- `Elb`
- `Alb`
- `StepFunction`
- `DynamoDb` (new implementation of the existing non-cloudformation mechanism)
- `Sqs` (new implementation of the existing non-cloudformation mechanism)
- `VpcSubnet` (this is a custom service using JUST EAT custom metrics)
- `ElastiCache`

## Alarm names and default thresholds

For each resource each of the default alarms will be applied. See [alarm definitions](Watchman.Engine/Alarms/Defaults.cs) for more details of periods etc.

### Rds

- `FreeStorageSpaceLow`: 30 (%)
- `CPUUtilizationHigh`: 60 (%)
- `DatabaseConnectionsHigh`: 200 (count)

### AutoScaling
#### Values

- `CPUCreditBalanceLow`: 0.2 (credit balance)
- `CPUUtilizationHigh`: 90 (%)
- `GroupInServiceInstancesLow`: 50 (% of desired)

#### Options
- `InstanceCountIncreaseDelayMinutes` Use to delay increasing the minimum threshold when the instance count increases (e.g. when scaling). The value used it then obtained from CloudWatch - using the minimum of `GroupDesiredCapacity` over the time period specified. Note that if CloudWatch metrics are not present then the current Desired capacity is used, as it better to have a more sensitive alarm than none.

### Lambda

- `ErrorsHigh`: 3 (count)
- `DurationHigh`: 50 (% of defined Timeout)
- `ThrottlesHigh`: 5 (count)
- `InvocationsLow`: Disabled by default. If enabled it checks the function was executed at least once in a day.
- `InvocationsHigh`: Disabled by default. If enabled it checks the function was executed at most x in a day.

### Kinesis

- `ReadProvisionedThroughputExceededHigh`: 1 (count)
- `WriteProvisionedThroughputExceededHigh`: 1 (count)

### StepFunction

- `ExecutionsFailedHigh`: 1 (count)

### DynamoDb

- `ConsumedReadCapacityUnitsHigh`: 80 (% of provisioned)
- `ConsumedWriteCapacityUnitsHigh`: 80 (% of provisioned)
- `ReadThrottleEventsHigh`: 2 
- `WriteThrottleEventsHigh`: 2 
- `GsiConsumedReadCapacityUnitsHigh`: 80 (%)
- `GsiConsumedWriteCapacityUnitsHigh`: 80 (%)
- `GsiReadThrottleEventsHigh`: 2
- `GsiWriteThrottleEventsHigh`: 2

#### Options
- `MonitorWrites` Shorthand can be used to disable all the write alarms. Default is `true`.
- `ThresholdIsAbsolute` If this value is true, the `Consumed` metrics above will use this value as an absolute, instead of the value calculated as a percentage of the provisioned capacity. The value is measured over a period of 60 seconds, so if you want a threshold on WCU / RCU, multiply your desired value by 60. Example: Desired alarm on RCU of 200, so threshold over 60 seconds is `12000`. Default value is `false`.

### Sqs

Sqs will mark any queue as an error queue if it ends with "_error"

- `NumberOfVisibleMessages`: 100
- `AgeOfOldestMessage`: 600 (seconds)
- `NumberOfVisibleMessages_Error`: 10
- `AgeOfOldestMessage_Error`: 600 (seconds)

#### Options

- `IncludeErrorQueues`: Enable special handling of _error queues matching the primary queue name (default `true`)

### VpcSubnets

- `IpAddressesRemainingLow` 30 (% of subnet allocation)

### Elb

Note that using the defaults here for all alarms is probably not that useful.

- `Elb5xxErrorsHigh`: 50 (count)
- `Http5xxErrorsHigh`: 500 (count)
- `SurgeQueueLengthHigh`: 200 (count)
- `SpilloverCountHigh`: 10 (count)
- `LatencyHigh`: 0.5 (average in seconds)
- `UnHealthyHostCountHigh`: 1 (count)

### Alb

- `5xxErrorsHigh`: 10 (count)
- `Target5xxErrorsHigh`: 10 (count)
- `RejectedConnectionCountHigh`: 10 (count)
- `TargetResponseTimeHigh`: 2 (seconds) compared against `p99`

### ElastiCache

- `CPUUtilizationHigh`: 60 (%)
- `EvictionStarted`: 1

## Full example

```
{
	"Name": "TEST",
  	"AlarmNameSuffix": "TestingGroup",
  	"Targets": [
    	{ "Email": "email@just-eat.com" }
  	],
  	"Services": {
    	"Rds": { 
    		"Resources": [{
       			"Pattern": "allorest", 
        		"Thresholds": {
        			"FreeStorageSpaceLow": 20
        		}
    		},{
    			"Name": "something"
    		},{
    			"Name": "another"
    		}],
    		"Values": {
      			"FreeStorageSpaceLow": 31
      		}
	    }
	    "Lambda": {
      		"Resources": [
      			{"Pattern": "api-"}
      		],
      		"Values": {
      			"ErrorsHigh": 10
      		}
	    }
    }
}
```
