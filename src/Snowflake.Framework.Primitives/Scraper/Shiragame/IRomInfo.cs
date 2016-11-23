using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Shiragame
{
    /// <summary>
    /// Represents a shiragame hashed info
    /// </summary>
    public interface IRomInfo
    {
        /// <summary>
        /// The Stone platform ID
        /// </summary>
        string PlatformId { get; }

        /// <summary>
        /// The canonical CRC32 from the dat file
        /// </summary>
        string CRC32 { get; }

        /// <summary>
        /// The canonical MD5 from the dat file
        /// </summary>
        string MD5 { get; }

        /// <summary>
        /// The canonical SHA1 from the dat file
        /// </summary>
        string SHA1 { get; }

        /// <summary>
        /// The mimetype of the file
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// The canonical filename from the dat file
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 region code for this rom
        /// </summary>
        string Region { get; }
    }
}
