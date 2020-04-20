using System;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    /// <summary>
    /// A seed is a unit of data with a type and a string value, with a unique
    /// ID and a parent ID. Within an <see cref="ISeedRootContext"/>, a list of seeds
    /// form a tree structure that can be traversed to produce records and other
    /// resultant data from scraped metadata and media.
    /// </summary>
    public interface ISeed
    {
        /// <summary>
        /// The source for a root seed.
        /// </summary>
        public static readonly string RootSource = "__root";

        /// <summary>
        /// The source if the data was sourced from sources other than scrapers.
        /// </summary>
        public static readonly string ClientSource = "__client";

        /// <summary>
        /// Gets the unique GUID of the seed.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Gets the unique GUID of this seeds parent.
        /// The relation between parent and child seeds exists only within
        /// a <see cref="ISeedRootContext"/>.
        /// </summary>
        Guid Parent { get; }

        /// <summary>
        /// Gets the type and value content of the seed.
        /// </summary>
        SeedContent Content { get; }

        /// <summary>
        /// Gets the source from which this seed's data was sourced from
        /// or created. By convention, if the data was sourced from
        /// external sources other than scrapers, the source should be
        /// the string "__client".
        /// </summary>
        string Source { get; }
    }
}
