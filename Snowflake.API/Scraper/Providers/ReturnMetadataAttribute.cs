using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Providers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReturnMetadataAttribute : Attribute
    {
        public string ReturnMetadata { get; }
        public ReturnMetadataAttribute(string returnMetadata)
        {
            this.ReturnMetadata = returnMetadata;
        }
    }
}
