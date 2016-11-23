using Snowflake.Records.Metadata;

namespace Snowflake.Romfile
{
    /// <summary>
    /// Represents information retrieved from the contents of a rom through File Signatures
    /// <see cref="IFileSignature"/>
    /// </summary>
    public interface IRomFileInfo
    {
        /// <summary>
        /// The stone mimetype of the ROM. 
        /// </summary>
        string Mimetype { get; }
        /// <summary>
        /// The serial of the ROM, if available
        /// </summary>
        string Serial { get; }
        /// <summary>
        /// The internal name of the ROM, if available
        /// </summary>
        string InternalName { get; }
    }
}