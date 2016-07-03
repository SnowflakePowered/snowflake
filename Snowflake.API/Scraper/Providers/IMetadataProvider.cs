using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    public interface IMetadataProvider: IScrapeProvider<IScrapeResult>, IPlugin
    { 
    }
}
