using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.ElastiCache;
using Amazon.ElastiCache.Model;

namespace Watchman.AwsResources.Services.ElastiCache
{
    public class ElastiCacheSource : ResourceSourceBase<CacheNode>
    {
        private readonly IAmazonElastiCache _amazonElastiCache;

        public ElastiCacheSource(IAmazonElastiCache amazonElastiCache)
        {
            _amazonElastiCache = amazonElastiCache;
        }

        protected override async Task<IEnumerable<CacheNode>> FetchResources()
        {
            var results = new List<IEnumerable<CacheNode>>();
            string marker = null;

            do
            {
                var response = await _amazonElastiCache.DescribeCacheClustersAsync(new DescribeCacheClustersRequest
                {
                    Marker = marker
                });

                results.Add(response.CacheClusters.SelectMany(x => x.CacheNodes));
                marker = response.Marker;
            }
            while (!string.IsNullOrEmpty(marker));

            return results.SelectMany(x => x).ToList();
        }

        protected override string GetResourceName(CacheNode resource) => resource.CacheNodeId;
    }
}
