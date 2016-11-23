using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Represents a provider for queried data requiring a search query and a platform.
    /// </summary>
    /// <remarks>
    /// A query provider should only implement <see cref="QueryBestMatch(string, string)"/> and <see cref="Query(string, string)"/>
    /// Other types of queries are implemented as functions that take in a single <see cref="IMetadataCollection"/>,
    /// and be marked with a <see cref="ProviderAttribute"/>. Functions that require a certain metadata to be available should mark such
    /// metadata as <see cref="RequiredMetadataAttribute"/>
    /// </remarks>
    /// <see cref="ProviderAttribute"/>
    /// <typeparam name="T">The type of queried data returned</typeparam>
    public interface IQueryProvider<out T>
    {
        /// <summary>
        /// Query a single best match given the restraints of a search query and a platform restrictor
        /// </summary>
        /// <param name="searchQuery">The query to process</param>
        /// <param name="platformId">Limit results to a single stone platform ID</param>
        /// <returns></returns>
        T QueryBestMatch(string searchQuery, string platformId);
        /// <summary>
        /// Gets all results from a source
        /// </summary>
        /// <param name="searchQuery">The query to process</param>
        /// <param name="platformId">Limit results to a single stone platform ID</param>
        /// <returns>Results returned from the query</returns>
        IEnumerable<T> Query(string searchQuery, string platformId);
        /// <summary>
        /// Queries a match given the available metadata, from the most well suited query function given the 
        /// available keys in the metadata collection
        /// </summary>
        /// <param name="metadata">The metadata collection to query with</param>
        /// <see cref="ProviderAttribute"/>
        /// <seealso cref="RequiredMetadataAttribute"/>
        /// <seealso cref="ReturnMetadataAttribute"/>
        /// <returns>Results returned from the query.</returns>
        IEnumerable<T> Query(IMetadataCollection metadata);
    }
}
