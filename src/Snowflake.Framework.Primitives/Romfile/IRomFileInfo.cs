namespace Snowflake.Romfile
{
    /// <summary>
    /// Represents information retrieved from the contents of a rom through File Signatures
    /// <see cref="IFileSignature"/>
    /// </summary>
    public interface IRomFileInfo
    {
        /// <summary>
        /// Gets the stone mimetype of the ROM.
        /// </summary>
        string Mimetype { get; }

        /// <summary>
        /// Gets the serial of the ROM, if available
        /// </summary>
        string Serial { get; }

        /// <summary>
        /// Gets the internal name of the ROM, if available
        /// </summary>
        string InternalName { get; }
    }
}
