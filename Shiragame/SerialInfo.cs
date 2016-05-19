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
        /// The Stone platform ID
        /// </summary>
        public string PlatformId { get; }
        /// <summary>
        /// The canonical game name 
        /// </summary>
        public string GameName { get; }
        /// <summary>
        /// The region code
        /// </summary>
        public string Region { get; }
        /// <summary>
        /// The game serials
        /// </summary>
        public IEnumerable<string> Serials { get; }


        internal SerialInfo(string platformId, string gameName, string region, IEnumerable<string> serials)
        {
            this.PlatformId = platformId;
            this.GameName = gameName;
            this.Serials = serials.ToList();
            this.Region = region;
        }
    }
}
