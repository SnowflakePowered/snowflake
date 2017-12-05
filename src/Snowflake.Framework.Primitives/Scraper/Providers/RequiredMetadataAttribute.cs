using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Guarantees that only <see cref="IMetadataCollection"/> with such metadata will be passed to this function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequiredMetadataAttribute : Attribute
    {
        /// <summary>
        /// Gets the key of the metadata that is required
        /// </summary>
        public string Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredMetadataAttribute"/> class.
        /// Guarantees that only <see cref="IMetadataCollection"/> with such metadata will be passed to this function.
        /// </summary>
        /// <param name="metadata">The key of the metadata that is required</param>
        public RequiredMetadataAttribute(string metadata)
        {
            this.Metadata = metadata;
        }
    }
}
