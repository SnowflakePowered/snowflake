using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin;

namespace Snowflake.Romfile
{
    /// <summary>
    /// Represents the ROM file signature
    /// </summary>
    public interface IFileSignature : IBasePlugin
    {
        /// <summary>
        /// The file extensions for this file signature comparator
        /// Usually the same if only one method is used.
        /// </summary>
        /// <example>
        /// .iso, .cso, .wbfs for Wii games
        /// </example>
        IList<string> FileExtensions { get; }
        /// <summary>
        /// The byte array from byte position 0 containing the header or other identifier of the ROM.
        /// Usually the first 1024 bytes.
        /// </summary>
        byte[] HeaderSignature { get; }
        /// <summary>
        /// The offset from byte position 0 to the start of the header. 
        /// IFileSignature.HeaderSignature[IFileSignature.HeaderOffset] should return the first byte of the header
        /// </summary>
        long HeaderOffset { get; }
        /// <summary>
        /// The Stone Platform ID this file signature uses
        /// </summary>
        string SupportedPlatform{ get; }
        /// <summary>
        /// Whether or not the filename's file extension is in IFileSignature.FileExtensions
        /// </summary>
        /// <param name="fileName">The filename of the ROM</param>
        /// <returns>Whether or not this filename's file extension is valid for this type of platform</returns>
        bool FileExtensionMatches(string fileName);
        /// <summary>
        /// Whether or not the header signature of a file matches this platform's ROM type.
        /// To handle multiple types of ROMs, use a series of ifs or an switch.
        /// </summary>
        /// <param name="fileName">The filename of the ROM</param>
        /// <returns>If this ROM is executable data for this platform, it should return true.</returns>
        bool HeaderSignatureMatches(string fileName);
        /// <summary>
        /// Gets the game id from the file signature if possible
        /// <param name="fileName">The filename of the ROM</param>
        /// </summary>
        string GetGameId(string fileName);
  
    }
}
