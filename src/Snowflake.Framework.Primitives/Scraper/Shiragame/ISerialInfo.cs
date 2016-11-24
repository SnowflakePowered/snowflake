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
        /// The Stone platform ID
        /// </summary>
        string PlatformId { get; }

        /// <summary>
        /// The canonical game name 
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The region code
        /// </summary>
        string Region { get; }

        /// <summary>
        /// The game serials
        /// </summary>
        string Serial { get; }
    }  
    
}
