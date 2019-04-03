using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CloudWatch.Model;
using Amazon.ElastiCache.Model;
using Watchman.Configuration.Generic;

namespace Watchman.AwsResources.Services.ElastiCache
{
    public class ElastiCacheAlarmDataProvider : IAlarmDimensionProvider<CacheNode>,
        IResourceAttributesProvider<CacheNode, ResourceConfig>
    {
        public List<Dimension> GetDimensions(CacheNode resource, IList<string> dimensionNames)
        {
            return dimensionNames
                .Select(x => GetDimension(resource, x))
                .ToList();
        }

        public Task<decimal> GetValue(CacheNode resource, ResourceConfig config, string property)
        {
            throw new System.NotImplementedException();
        }

        private Dimension GetDimension(CacheNode resource, string dimensionName)
        {
            var dim = new Dimension
            {
                Name = dimensionName
            };

            switch (dimensionName)
            {
                case "CacheNodeId":
                    dim.Value = resource.CacheNodeId;
                    break;

                default:
                    throw new Exception("Unsupported dimension " + dimensionName);
            }

            return dim;
        }
    }
}
