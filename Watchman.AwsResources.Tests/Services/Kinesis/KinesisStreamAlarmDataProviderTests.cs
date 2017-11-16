using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CloudWatch.Model;
using NUnit.Framework;
using Watchman.AwsResources.Services.Kinesis;

namespace Watchman.AwsResources.Tests.Services.Kinesis
{
    [TestFixture]
    public class KinesisStreamAlarmDataProviderTests
    {
        private KinesisStreamData _streamData;
        private KinesisStreamAlarmDataProvider _streamDataProvider;

        [SetUp]
        public void Setup()
        {
            _streamData = new KinesisStreamData
            {
                Name = "Stream Name"
            };

            _streamDataProvider = new KinesisStreamAlarmDataProvider();
        }

        [Test]
        public void GetDimensions_KnownDimensions_ReturnsValue()
        {
            //arange

            //act
            var result = _streamDataProvider.GetDimensions(_streamData, new List<string> { "StreamName" });

            //assert
            Assert.That(result.Count, Is.EqualTo(1));

            var dim = result.Single();
            Assert.That(dim.Value, Is.EqualTo(_streamData.Name));
            Assert.That(dim.Name, Is.EqualTo("StreamName"));
        }

        [Test]
        public void GetDimensions_UnknownDimension_ThrowException()
        {
            //arange

            //act
            List<Dimension> TestDelegate() => _streamDataProvider.GetDimensions(_streamData, new List<string> { "UnknownDimension" });

            //assert
            Assert.That(TestDelegate, Throws.TypeOf<Exception>()
                .With.Message.EqualTo("Unsupported dimension UnknownDimension"));
        }

        [Test]
        public void GetAttribute_UnknownAttribute_ThrowException()
        {
            //arange

            //act
            decimal TestDelegate() => _streamDataProvider.GetValue(_streamData, "Unknown Attribute");

            //assert
            Assert.That(TestDelegate, Throws.TypeOf<NotImplementedException>());
        }
    }
}
