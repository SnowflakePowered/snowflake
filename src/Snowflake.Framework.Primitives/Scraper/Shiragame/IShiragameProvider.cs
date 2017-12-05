using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Shiragame
{
    /// <summary>
    /// Represents a Shiragame information database
    /// </summary>
    public interface IShiragameProvider
    {
        /// <summary>
        /// Gets the version of Stone used in generating the database
        /// </summary>
        Version StoneVersion { get; }

        /// <summary>
        /// Gets the database version
        /// </summary>
        Version DatabaseVersion { get; }

        /// <summary>
        /// Checks if the file is a mame ROM
        /// </summary>
        /// <param name="mameRom">The name of the file, including .zip extension</param>
        /// <returns>Whether the filename matches with a known mame dump</returns>
        bool IsMameRom(string mameRom);

        /// <summary>
        /// Get ROM information from the CRC32 string of a file.
        /// CRC32 has conflicts, use MD5 or SHA1 if possible.
        /// </summary>
        /// <param name="crc32">The CRC32 of the file</param>
        /// <returns>The rom information</returns>
        IRomInfo GetFromCrc32(string crc32);

        /// <summary>
        /// Get ROM informatiom from the MD5 of a file
        /// </summary>
        /// <param name="md5">The MD5 of the file</param>
        /// <returns>The rom information</returns>
        IRomInfo GetFromMd5(string md5);

        /// <summary>
        /// Get ROM information from the SHA1 of a file
        /// </summary>
        /// <param name="sha1">The SHA1 of the file</param>
        /// <returns>The rom information</returns>
        IRomInfo GetFromSha1(string sha1);

        /// <summary>
        /// Gets serial information, given the known platform ID and a known serial.
        /// Platform ID must be known, as serials conflict within platforms.
        /// </summary>
        /// <param name="platformId">The Stone Platform ID for the serial</param>
        /// <param name="serial">The serial from the file</param>
        /// <returns>The serial information</returns>
        ISerialInfo GetFromSerial(string platformId, string serial);
    }
}
