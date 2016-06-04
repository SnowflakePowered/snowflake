using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Provider;

namespace Snowflake.Scraper.Providers
{
    public abstract class MetadataProvider : IMetadataProvider
    {
        private readonly IList<ProviderQueryFunction<IScrapedMetadataCollection>> cachedFunctions;
        protected MetadataProvider()
        {
            this.cachedFunctions = this.CacheFunctions();
        }
        public abstract IList<IScrapedMetadataCollection> QueryAllResults(string searchQuery, string platformId);

        public abstract IScrapedMetadataCollection QueryBestMatch(string searchQuery, string platformId);
        
        public IScrapedMetadataCollection Query(IMetadataCollection metadata)
        {
            var result = (from p in this.cachedFunctions
                    where metadata.Keys.Intersect(p.RequiredMetadata).Count() == p.RequiredMetadata.Count()
                    let res = p.Query(metadata)
                    where !(res as IMetadataCollection).Keys.Except(p.ReturnMetadata).Any()
                    select res).FirstOrDefault();
            if (result == null) throw new InvalidOperationException("Scraper did not return expected parameters");
            return result;
        }

        public IScrapedMetadataCollection Query(IMetadataCollection metadata, IList<string> wantedMetadata)
        {
            return (from p in this.cachedFunctions
                    where metadata.Keys.Intersect(p.RequiredMetadata).Count() == p.RequiredMetadata.Count()
                    where p.ReturnMetadata.Intersect(wantedMetadata).Count() == wantedMetadata.Count()
                    select p.Query(metadata)).FirstOrDefault();
        }

        private IList<ProviderQueryFunction<IScrapedMetadataCollection>> CacheFunctions()
        {
            return (from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                where Attribute.IsDefined(m, typeof(ProviderAttribute))
                let returnValues = (from attr in Attribute.GetCustomAttributes(m, typeof(ReturnMetadataAttribute))
                    select ((ReturnMetadataAttribute) attr).Metadata)
                let requiredValues = (from attr in Attribute.GetCustomAttributes(m, typeof(RequiredMetadataAttribute))
                    select ((RequiredMetadataAttribute) attr).Metadata)
                where returnValues.Any()
                where requiredValues.Any()
                let func = ProviderQueryFunction<IScrapedMetadataCollection>.Make(this, m)
                select new ProviderQueryFunction<IScrapedMetadataCollection>(requiredValues, returnValues, func)).ToList();
        }
    }
}
