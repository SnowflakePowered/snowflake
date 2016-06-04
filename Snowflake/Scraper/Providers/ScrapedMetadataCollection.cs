using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Provider;

namespace Snowflake.Scraper.Providers
{
    public class ScrapedMetadataCollection : MetadataCollection, IScrapedMetadataCollection
    {
        public ScrapedMetadataCollection(string scraperId, double accuracy) : base(Guid.NewGuid())
        {
            this.ScraperId = scraperId;
            this.Accuracy = accuracy;
        }

        public string ScraperId { get; }
        public double Accuracy { get; }
        public string Title
        {
            get { return this[$"{this.ScraperId}_scraped_title"].Value; }
            set { (this as IMetadataCollection)[$"{this.ScraperId}_scraped_title"] = value; }
        }
        

    }
}
