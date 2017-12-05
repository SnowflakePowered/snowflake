using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Represents a source of query providers
    /// </summary>
    public interface IQueryProviderSource
    {
        /// <summary>
        /// Gets providers metadata providers
        /// </summary>
        IEnumerable<IQueryProvider<IScrapedMetadataCollection>> MetadataProviders { get; }

        /// <summary>
        /// Gets provides media providers in the form of file records to be attached to a game.
        /// </summary>
        IEnumerable<IQueryProvider<IList<IFileRecord>>> MediaProviders { get; }

        /// <summary>
        /// Registers a metadata provider
        /// </summary>
        /// <param name="queryProvider">The provider to register</param>
        void Register(IQueryProvider<IScrapedMetadataCollection> queryProvider);

        /// <summary>
        /// Registers a media provider
        /// </summary>
        /// <param name="queryProvider">The provider to register</param>
        void Register(IQueryProvider<IList<IFileRecord>> queryProvider);
    }
}
