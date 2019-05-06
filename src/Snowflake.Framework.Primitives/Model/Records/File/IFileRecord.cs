using Snowflake.Filesystem;

namespace Snowflake.Model.Records.File
{
    /// <summary>
    /// <para>
    /// Represents a file on disk.
    /// </para>
    /// <para>
    /// The only difference between an <see cref="IFile"/> and an <see cref="IFileRecord"/>
    /// is that the mimetypf of an <see cref="IFileRecord"/> must be known. If so, then 
    /// metadata can be recorded for it within a <see cref="IMetadataCollection"/>, relative to
    /// the manifested <see cref="IFile"/> it wraps.
    /// </para>
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
