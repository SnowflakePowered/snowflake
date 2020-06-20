using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;

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
        /// Never close <paramref name="fileContents"/>.
        /// This method must never throw. Return false if an exception occurs.
        /// </remarks>
        /// <param name="fileContents">The contents of the ROM</param>
        /// <returns>If this ROM is executable data for this platform, it should return true.</returns>
        bool HeaderSignatureMatches(Stream fileContents);

        /// <summary>
        /// Gets the game serial from the file signature if possible
        /// </summary>
        /// <remarks>
        /// Never close <paramref name="fileContents"/>.
        /// 
        /// This method may not throw if <see cref="HeaderSignatureMatches(Stream)"/> is true.
        /// </remarks>
        /// <param name="fileContents">The contents of the ROM</param>
        string? GetSerial(Stream fileContents);

        /// <summary>
        /// Gets the internal name of the ROM if possible
        /// </summary>
        /// <remarks>
        /// Never close <paramref name="fileContents"/>. 
        /// 
        /// This method may not throw if <see cref="HeaderSignatureMatches(Stream)"/> is true.
        /// </remarks>
        /// <param name="fileContents">The contents of the ROM</param>
        string? GetInternalName(Stream fileContents);
    }
}
