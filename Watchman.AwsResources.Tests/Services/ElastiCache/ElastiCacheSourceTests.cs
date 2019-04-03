using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.ElastiCache;
using Amazon.ElastiCache.Model;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Moq;
using NUnit.Framework;
using Watchman.AwsResources.Services.ElastiCache;
using Watchman.AwsResources.Services.StepFunction;

namespace Watchman.AwsResources.Tests.Services.ElastiCache
{
    [TestFixture]
    public class ElastiCacheSourceTests
    {
        private ElastiCacheSource _source;
        private DescribeCacheClustersResponse _firstPage;
        private DescribeCacheClustersResponse _secondPage;
        private DescribeCacheClustersResponse _thirdPage;

        [SetUp]
        public void SetUp()
        {
            var stepClusterMock = new Mock<IAmazonElastiCache>();

            _firstPage = new DescribeCacheClustersResponse
            {
                Marker = "token-1",
                CacheClusters = new List<CacheCluster>
                {
                    new CacheCluster
                    {
                        CacheNodes = new List<CacheNode>()
                        {
                            new CacheNode
                            {
                                CacheNodeId = "CacheNodeId - 1"
                            }
                        }
                    }
                }
            };

            _secondPage = new DescribeCacheClustersResponse
            {
                Marker = "token-2",
                CacheClusters = new List<CacheCluster>
                {
                    new CacheCluster
                    {
                        CacheNodes = new List<CacheNode>()
                        {
                            new CacheNode
                            {
                                CacheNodeId = "CacheNodeId - 2"
                            }
                        }
                    }
                }
            };


            _thirdPage = new DescribeCacheClustersResponse
            {
                CacheClusters = new List<CacheCluster>
                {
                    new CacheCluster
                    {
                        CacheNodes = new List<CacheNode>()
                        {
                            new CacheNode
                            {
                                CacheNodeId = "CacheNodeId - 3"
                            }
                        }
                    }
                }
            };

            stepClusterMock.Setup(c => c.DescribeCacheClustersAsync(
                It.Is<DescribeCacheClustersRequest>(r => r.Marker == null)
                , It.IsAny<CancellationToken>()))
                .ReturnsAsync(_firstPage);

            stepClusterMock.Setup(c => c.DescribeCacheClustersAsync(
                    It.Is<DescribeCacheClustersRequest>(r => r.Marker == "token-1")
                    , It.IsAny<CancellationToken>()))
                .ReturnsAsync(_secondPage);

            stepClusterMock.Setup(c => c.DescribeCacheClustersAsync(
                    It.Is<DescribeCacheClustersRequest>(r => r.Marker == "token-2")
                    , It.IsAny<CancellationToken>()))
                .ReturnsAsync(_thirdPage);

            _source = new ElastiCacheSource(stepClusterMock.Object);
        }

        [Test]
        public async Task GetResourceNamesAsync_MultiplePages_AllFetchedAndReturned()
        {
            // act
            var result = await _source.GetResourceNamesAsync();

            // assert
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result.First(), Is.EqualTo(_firstPage.CacheClusters.Single().CacheNodes.Single().CacheNodeId));
            Assert.That(result.Skip(1).First(), Is.EqualTo(_secondPage.CacheClusters.Single().CacheNodes.Single().CacheNodeId));
            Assert.That(result.Skip(2).First(), Is.EqualTo(_thirdPage.CacheClusters.Single().CacheNodes.Single().CacheNodeId));
        }

        [Test]
        public async Task GetResourceNamesAsync_SinglePage_FetchedAndReturned()
        {
            // arrange
            _firstPage.Marker = null;

            // act
            var result = await _source.GetResourceNamesAsync();

            // assert
            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result.First(), Is.EqualTo(_firstPage.CacheClusters.Single().CacheNodes.Single().CacheNodeId));
        }

        [Test]
        public async Task GetResourceNamesAsync_EmptyResult_EmptyListReturned()
        {
            // arrange
            _firstPage.Marker = null;
            _firstPage.CacheClusters = new List<CacheCluster>();

            // act
            var result = await _source.GetResourceNamesAsync();

            // assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetResouceAsync_ReturnsCorrectResource()
        {
            // arrange
            var secondCacheNodeId = _secondPage.CacheClusters.Single().CacheNodes.Single().CacheNodeId;

            // act
            var result = await _source.GetResourceAsync(secondCacheNodeId);

            // assert
            Assert.That(result.Name, Is.EqualTo(secondCacheNodeId));
            Assert.That(result.Resource, Is.InstanceOf<CacheNode>());
            Assert.That(result.Resource.CacheNodeId, Is.EqualTo(secondCacheNodeId));
        }
    }
}
