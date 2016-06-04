using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Providers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequiredMetadataAttribute : Attribute
    {
        public string Metadata { get; }
        public RequiredMetadataAttribute(string metadata)
        {
            this.Metadata = metadata;
        }
    }
}
