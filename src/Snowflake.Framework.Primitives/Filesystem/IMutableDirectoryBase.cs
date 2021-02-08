using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Base interface for a mutable directory.
    /// 
    /// Never refer to this interface directly. The most basic directory type is <see cref="IDirectory"/>.
    /// The most basic directory type is <see cref="IDirectory"/>.
    /// </summary>
    public interface IMutableDirectoryBase
    {
        /// <summary>
        /// Gets the name of the directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Copies a file from an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// Do not use this method to copy from a managed <see cref="IDirectory"/>.
        /// Instead, use <see cref="CopyFrom(IReadOnlyFile)"/>.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(FileInfo source);

        /// <summary>
        /// Copies a file from an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// Do not use this method to copy from a managed <see cref="IDirectory"/>.
        /// Instead, use <see cref="CopyFrom(IReadOnlyFile)"/>.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(FileInfo source, bool overwrite);

        /// <summary>
        /// Copies a file asynchronously from an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// Do not use this method to copy from a managed <see cref="IDirectory"/>.
        /// Instead, use <see cref="CopyFrom(IReadOnlyFile)"/>.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="cancellation">A cancellation token that is forwarded to the underlying <see cref="Task{TResult}"/>.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(FileInfo source, CancellationToken cancellation = default);

        /// <summary>
        /// Copies a file asynchronously from an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// Do not use this method to copy from a managed <see cref="IDirectory"/>.
        /// Instead, use <see cref="CopyFrom(IReadOnlyFile)"/>.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <param name="cancellation">A cancellation token that is forwarded to the underlying <see cref="Task{TResult}"/>.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation = default);

        /// <summary>
        /// Copies a file from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(IReadOnlyFile source);

        /// <summary>
        /// Copies a file from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(IReadOnlyFile source, bool overwrite);

        /// <summary>
        /// Copies a file asynchronously from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="cancellation">Cancellation token for the asynchronous task.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDeletableDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(IReadOnlyFile source, CancellationToken cancellation = default);

        /// <summary>
        /// Copies a file asynchronously from a <see cref="IFile"/> from another <see cref="IDeletableDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDeletableDirectory"/></param>
        /// <param name="cancellation">Cancellation token for the asynchronous task.</param>
        /// <returns>The <see cref="IReadOnlyFile"/> that describes the file in the current <see cref="IDeletableDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(IReadOnlyFile source, bool overwrite, CancellationToken cancellation = default);

        /// <summary>
        /// Whether or not this directory contains a file or directory in its manifest. If provided a
        /// full path, this will truncate the path using <see cref="Path.GetFileName(string)"/>
        /// </summary>
        /// <param name="file">The name of the file to check.</param>
        /// <returns>Whether or not this directory contains the given file or directory.</returns>
        bool ContainsFile(string file);

        /// <summary>
        /// Whether or not this directory contains directory as a direct child.
        /// </summary>
        /// <param name="directory">The name of the directory to check.</param>
        /// <returns>Whether or not this directory contains the given directory.</returns>
        bool ContainsDirectory(string directory);

        /// <summary>
        /// Gets the underlying <see cref="DirectoryInfo"/> where files are contained.
        /// 
        /// This is very rarely necessary, and most IO tasks can be done efficiently and safely using the
        /// provided API. Manipulating the underlying file system will potentially desync the manifest,
        /// and cause loss of associated metadata.
        /// </summary>
        /// <returns>The underlying <see cref="DirectoryInfo"/> where files are contained.</returns>
        [Obsolete("Avoid accessing the underlying file path, and use the object methods instead.")]
        DirectoryInfo UnsafeGetPath();
    }
}
