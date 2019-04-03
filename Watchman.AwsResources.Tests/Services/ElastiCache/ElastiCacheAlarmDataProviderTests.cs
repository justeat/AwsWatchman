using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CloudWatch.Model;
using Amazon.ElastiCache.Model;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Watchman.AwsResources.Services.ElastiCache;

namespace Watchman.AwsResources.Tests.Services.ElastiCache
{
    [TestFixture]
    public class ElastiCacheAlarmDataProviderTests
    {
        private CacheNode _node;
        private ElastiCacheAlarmDataProvider _dataProvider;

        [SetUp]
        public void SetUp()
        {
            _node = new CacheNode
            {
                CacheNodeId = "Existing Cache Node 1"
            };

            _dataProvider = new ElastiCacheAlarmDataProvider();
        }

        [Test]
        public void GetDimensions_KnownDimensions_ReturnsValue()
        {
            //act
            var result = _dataProvider.GetDimensions(_node, new List<string> { nameof(CacheNode.CacheNodeId) });

            //assert
            Assert.That(result.Count, Is.EqualTo(1));

            var dim = result.Single();
            Assert.That(dim.Value, Is.EqualTo(_node.CacheNodeId));
            Assert.That(dim.Name, Is.EqualTo(nameof(CacheNode.CacheNodeId)));
        }

        [Test]
        public void GetDimensions_UnknownDimension_ThrowException()
        {
            //act
            ActualValueDelegate<List<Dimension>> testDelegate =
                () => _dataProvider.GetDimensions(_node, new List<string> { "UnknownDimension" });

            //assert
            Assert.That(testDelegate, Throws.TypeOf<Exception>()
                .With.Message.EqualTo("Unsupported dimension UnknownDimension"));
        }
    }
}
