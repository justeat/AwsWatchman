using System.Threading.Tasks;
using Amazon;
using Amazon.ElastiCache;
using NUnit.Framework;
using TestHelper;
using Watchman.AwsResources.Services.ElastiCache;
using Watchman.AwsResources.Services.StepFunction;

namespace Watchman.AwsResources.IntegrationTests.Service.StepFunction
{
    public class ElastiCacheSourceTests
    {
        [Test]
        public async Task GetResourceNamesAsyncShouldReturnResults()
        {
            var source = InitializeSource();

            var resources = await source.GetResourceNamesAsync();

            Assert.That(resources, Is.Not.Null);
            Assert.That(resources, Is.Not.Empty);
        }

        private ElastiCacheSource InitializeSource()
        {
            var amazonElastiCache = new AmazonElastiCacheClient(CredentialsReader.GetCredentials(), RegionEndpoint.EUWest1);
            return new ElastiCacheSource(amazonElastiCache);
        }
    }
}
