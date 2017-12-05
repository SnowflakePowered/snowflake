using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Shiragame
{
    /// <summary>
    /// Represents a datum matching a serial number to a game name
    /// </summary>
    public interface ISerialInfo
    {
        /// <summary>
        /// Gets the Stone platform ID
        /// </summary>
        string PlatformId { get; }

        /// <summary>
        /// Gets the canonical game name
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the region code
        /// </summary>
        string Region { get; }

        /// <summary>
        /// Gets the game serials
        /// </summary>
        string Serial { get; }
    }
}
