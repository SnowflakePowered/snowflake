using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Scraper;

namespace Snowflake.Romfile
{
    /// <summary>
    /// Represents the ROM file signature
    /// </summary>
    public interface IFileSignature
    {
        /// <summary>
        /// Gets the byte array from byte position 0 containing the header or other identifier of the ROM.
        /// Usually the first 1024 bytes.
        /// </summary>
        byte[] HeaderSignature { get; }

        /// <summary>
        /// Whether or not the header signature of a file matches this platform's ROM type.
        /// To handle multiple types of ROMs, use a series of ifs or an switch.
        /// </summary>
        /// <remarks>
        /// Never close <paramref name="fileContents"/>. Exceptions can be thrown freely without being handled,
        /// and will be treated as an invalid result (false).
        /// </remarks>
        /// <param name="fileContents">The contents of the ROM</param>
        /// <returns>If this ROM is executable data for this platform, it should return true.</returns>
        bool HeaderSignatureMatches(Stream fileContents);

        /// <summary>
        /// Gets the game serial from the file signature if possible
        /// <param name="fileContents">The contents of the ROM</param>
        /// </summary>
        /// <remarks>
        /// Never close <paramref name="fileContents"/>. Exceptions can be thrown freely without being handled,
        /// and will be treated as an invalid result (null).
        /// </remarks>
        string GetSerial(Stream fileContents);

        /// <summary>
        /// Gets the internal name of the ROM if possible
        /// <param name="fileContents">The contents of the ROM</param>
        /// </summary>
        /// <remarks>
        /// Never close <paramref name="fileContents"/>. Exceptions can be thrown freely without being handled,
        /// and will be treated as an invalid result (null).
        /// </remarks>
        string GetInternalName(Stream fileContents);
    }
}
