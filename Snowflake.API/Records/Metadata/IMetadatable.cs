using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    /// <summary>
    /// Represents a class that can be converted to metadata
    /// </summary>
    /// <typeparam name="T">The class to convert</typeparam>
    public interface IMetadatable<out T>
    {
        /// <summary>
        /// Converts the class into a metadatable representation, keyed on the metadata name.
        /// </summary>
        /// <returns>An enumerable of metadata</returns>
        IDictionary<string, IRecordMetadata> ToMetadata();

        /// <summary>
        /// Converts a collection of metadata to a class
        /// </summary>
        /// <param name="metadata">The metadata</param>
        /// <returns>The metadata</returns>
        T FromMetadata(IDictionary<string, IRecordMetadata> metadata);
    }
}
