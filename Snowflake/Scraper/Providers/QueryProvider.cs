using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    public abstract class QueryProvider<T> : IQueryProvider<T> 
    {
        private readonly IList<ProviderQueryFunction<T>> cachedFunctions;

        protected QueryProvider()
        {
            this.cachedFunctions = this.CacheFunctions();
        }

        public abstract IEnumerable<T> Query(string searchQuery, string platformId);

        public abstract T QueryBestMatch(string searchQuery, string platformId);

        public IEnumerable<T> Query(IMetadataCollection metadata)
        {
            return (from p in this.cachedFunctions.AsParallel()
                    where metadata.Keys.Intersect(p.RequiredMetadata).Count() == p.RequiredMetadata.Count()
                    let result = p.Query(metadata)
                    select result);
        }

        private IList<ProviderQueryFunction<T>> CacheFunctions()
        {
            return (from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    where Attribute.IsDefined(m, typeof(ProviderAttribute))
                    let returnValues = (from attr in Attribute.GetCustomAttributes(m, typeof(ReturnMetadataAttribute))
                                        select ((ReturnMetadataAttribute)attr).ReturnMetadata)
                    let requiredValues = (from attr in Attribute.GetCustomAttributes(m, typeof(RequiredMetadataAttribute))
                                          select ((RequiredMetadataAttribute)attr).Metadata)
                    where requiredValues.Any()
                    let func = ProviderQueryFunction<T>.Make(this, m)
                    select new ProviderQueryFunction<T>(requiredValues, returnValues, func)).ToList();
        }
    }
}
