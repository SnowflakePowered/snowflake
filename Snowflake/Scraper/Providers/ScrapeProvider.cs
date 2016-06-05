using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    public abstract class ScrapeProvider<T> : IScrapeProvider<T>
    {
        private readonly IList<ProviderQueryFunction<T>> cachedFunctions;

        protected ScrapeProvider()
        {
            this.cachedFunctions = this.CacheFunctions();
        }

        public abstract IList<T> QueryAllResults(string searchQuery, string platformId);

        public abstract T QueryBestMatch(string searchQuery, string platformId);

        public virtual T Query(IMetadataCollection metadata)
        {
            var result = (from p in this.cachedFunctions
                          where metadata.Keys.Intersect(p.RequiredMetadata).Count() == p.RequiredMetadata.Count()
                          select p.Query(metadata)).FirstOrDefault();
            return result;
        }

        public virtual T Query(IMetadataCollection metadata, params string[] wantedMetadata)
        {
            return (from p in this.cachedFunctions
                    where metadata.Keys.Intersect(p.RequiredMetadata).Count() == p.RequiredMetadata.Count()
                    where p.ReturnMetadata.Intersect(wantedMetadata).Count() == wantedMetadata.Count()
                    select p.Query(metadata)).FirstOrDefault();
        }

        private IList<ProviderQueryFunction<T>> CacheFunctions()
        {
            return (from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    where Attribute.IsDefined(m, typeof(ProviderAttribute))
                    let returnValues = (from attr in Attribute.GetCustomAttributes(m, typeof(ReturnMetadataAttribute))
                                        select ((ReturnMetadataAttribute)attr).ReturnMetadata)
                    let requiredValues = (from attr in Attribute.GetCustomAttributes(m, typeof(RequiredMetadataAttribute))
                                          select ((RequiredMetadataAttribute)attr).Metadata)
                    where returnValues.Any()
                    where requiredValues.Any()
                    let func = ProviderQueryFunction<T>.Make(this, m)
                    select new ProviderQueryFunction<T>(requiredValues, returnValues, func)).ToList();
        }
    }
}
