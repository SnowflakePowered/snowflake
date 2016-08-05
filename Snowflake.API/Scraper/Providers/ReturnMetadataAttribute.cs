using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Indicates what metadata is returned.
    /// Optional but recommended.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReturnMetadataAttribute : Attribute
    {
        /// <summary>
        /// The key of metadata to be returned
        /// </summary>
        public string ReturnMetadata { get; }
        /// <summary>
        /// Indicates what metadata is returned.
        /// Optional but recommended.
        /// </summary>
        /// <param name="returnMetadata">The key of metadata to be returned</param>
        public ReturnMetadataAttribute(string returnMetadata)
        {
            this.ReturnMetadata = returnMetadata;
        }
    }
}
