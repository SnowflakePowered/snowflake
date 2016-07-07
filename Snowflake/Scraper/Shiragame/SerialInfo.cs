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
    public class SerialInfo : ISerialInfo
    {
        /// <summary>
        /// The Stone platform ID
        /// </summary>
        public string PlatformId { get; }
        /// <summary>
        /// The canonical game name 
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// The region code
        /// </summary>
        public string Region { get; }
        /// <summary>
        /// The game serials
        /// </summary>
        public string Serial { get; }


        internal SerialInfo(string platformId, string serial, string title, string region)
        {
            this.PlatformId = platformId;
            this.Title = title;
            this.Serial = serial;
            this.Region = region;
        }
    }
}
