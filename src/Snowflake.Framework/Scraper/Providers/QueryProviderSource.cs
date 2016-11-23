using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;

namespace Snowflake.Scraper.Providers
{
    public class QueryProviderSource : IQueryProviderSource
    {
        public IEnumerable<IQueryProvider<IScrapedMetadataCollection>> MetadataProviders
            => this.metadataProviders.AsEnumerable();

        public IEnumerable<IQueryProvider<IList<IFileRecord>>> MediaProviders => 
            this.mediaProviders.AsEnumerable();

        private readonly IList<IQueryProvider<IScrapedMetadataCollection>> metadataProviders;
        private readonly IList<IQueryProvider<IList<IFileRecord>>>  mediaProviders;

        public QueryProviderSource()
        {
            this.metadataProviders = new List<IQueryProvider<IScrapedMetadataCollection>>();
            this.mediaProviders = new List<IQueryProvider<IList<IFileRecord>>>();
        }

        public void Register(IQueryProvider<IScrapedMetadataCollection> queryProvider)
        {
            this.metadataProviders.Add(queryProvider);
        }

        public void Register(IQueryProvider<IList<IFileRecord>> queryProvider)
        {
            this.mediaProviders.Add(queryProvider);
        }
    }
}
