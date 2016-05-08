using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    public interface IMetadatable<T>
    {
        /// <summary>
        /// Converts the class into a metadatable representation, keyed on the metadata name.
        /// </summary>
        /// <returns>An enumerable of metadata</returns>
        IDictionary<string, IMetadata> ToMetadata();

        /// <summary>
        /// Converts a collection of metadata to a class
        /// </summary>
        /// <param name="metadata">The metadata</param>
        /// <returns>The metadata</returns>
        T FromMetadata(IDictionary<string, IMetadata> metadata);
    }
}
