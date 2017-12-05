using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiragame
{
    /// <summary>
    /// Represents a datum matching a serial number to a game name
    /// </summary>
    public class SerialInfo
    {
        /// <summary>
        /// Gets the Stone platform ID
        /// </summary>
        public string PlatformId { get; }

        /// <summary>
        /// Gets the canonical game name
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the region code
        /// </summary>
        public string Region { get; }

        /// <summary>
        /// Gets the game serials
        /// </summary>
        public string Serial { get; }

        internal SerialInfo(string platformId, string title, string region, string serial)
        {
            this.PlatformId = platformId;
            this.Title = title;
            this.Serial = serial.Replace("-", string.Empty).Replace("_", string.Empty).ToUpperInvariant(); // normalize serial
            this.Region = region;
        }
    }
}
