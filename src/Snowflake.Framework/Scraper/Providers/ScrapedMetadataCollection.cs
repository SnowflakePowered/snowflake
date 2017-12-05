﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    public class ScrapedMetadataCollection : MetadataCollection, IScrapedMetadataCollection
    {
        public ScrapedMetadataCollection(string metadataSource, double accuracy)
            : base(Guid.NewGuid())
        {
            this.Source = metadataSource;
            this.Accuracy = accuracy;
        }

        /// <inheritdoc/>
        public string Source
        {
            get { return this["metadata_source"].Value; }
            set { (this as IMetadataCollection)["metadata_source"] = value; }
        }

        /// <inheritdoc/>
        public double Accuracy
        {
            get { return double.Parse(this["information_accuracy"].Value); }
            set { (this as IMetadataCollection)["information_accuracy"] = value.ToString(); }
        }
    }
}
