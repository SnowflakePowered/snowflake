using Snowflake.Filesystem;

namespace Snowflake.Model.Records.File
{
    /// <summary>
    /// Represents a file on disk.
    /// </summary>
    public interface IFileRecord : IRecord
    {
        /// <summary>
        /// Gets the mimetype of the file
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// The associated file of this file record.
        /// </summary>
        IFile File { get; }
    }
}
