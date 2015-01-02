using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Snowflake.Information.MediaStore
{
    /// <summary>
    /// Represents a section in the mediastore.
    /// </summary>
    public interface IMediaStoreSection
    {
        /// <summary>
        /// The name of the mediastore.
        /// </summary>
        string SectionName { get; set; }
        /// <summary>
        /// A dictionary holding all the items in the mediastore.
        /// Never access this dictionary directly, instead use the indexer.
        /// </summary>
        [Obsolete("Never access MediaStoreItems directly, instead use IMediaStoreSection indexer")]
        IReadOnlyDictionary<string, string> MediaStoreItems { get; }
        /// <summary>
        /// Add a file to the mediastore
        /// </summary>
        /// <param name="key">The key in which to store the item</param>
        /// <param name="value">The path to the file to be added</param>
        void Add(string key, string value);
        /// <summary>
        /// Remove a file in the mediastore
        /// </summary>
        /// <param name="key">The key of the mediastore item</param>
        void Remove(string key);
        /// <summary>
        /// Access a certain item in the mediastore
        /// </summary>
        /// <param name="key">The key of the mediastore item</param>
        /// <returns>The path to the mediastore item</returns>
        string this[string key] { get; set; }
    }
}
