using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Shiragame
{
    /// <summary>
    /// Represents a shiragame hashed info
    /// </summary>
    public interface IRomInfo
    {
        /// <summary>
        /// Gets the Stone platform ID
        /// </summary>
        string PlatformId { get; }

        /// <summary>
        /// Gets the canonical CRC32 from the dat file
        /// </summary>
        string CRC32 { get; }

        /// <summary>
        /// Gets the canonical MD5 from the dat file
        /// </summary>
        string MD5 { get; }

        /// <summary>
        /// Gets the canonical SHA1 from the dat file
        /// </summary>
        string SHA1 { get; }

        /// <summary>
        /// Gets the mimetype of the file
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Gets the canonical filename from the dat file
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the ISO 3166-1 alpha-2 region code for this rom
        /// </summary>
        string Region { get; }
    }
}
