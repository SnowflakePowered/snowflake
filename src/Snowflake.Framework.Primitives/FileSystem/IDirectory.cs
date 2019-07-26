using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents the root of a Directory, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Gets the name of the directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Opens an existing descendant directory with the given name.
        /// If the directory does not exist, creates the directory.
        /// You can open a nested directory using '/' as the path separator, and it 
        /// will be created relative to this current directory.
        /// </summary>
        /// <param name="name">The name of the existing directory</param>
        /// <returns>The directory if it exists, or null if it does not.</returns>
        IDirectory OpenDirectory(string name);

        /// <summary>
        /// Opens or creates a file, adding it to the manifest and assigning it a
        /// unique <see cref="Guid"/>.
        /// 
        /// Unlike <see cref="OpenDirectory(string)"/>, you can not use the path separator to
        /// open a nested file. Paths will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// Instead, use <see cref="OpenDirectory(string)"/> to open the directory of the desired file,
        /// then call <see cref="OpenFile(string)"/> on the returned instance.
        /// 
        /// Keep in mind this does not actually create a file on the underlying file system. 
        /// However, you can use <see cref="IFile.OpenStream()"/> and friends to create the file
        /// if it does not yet exist.
        /// </summary>
        /// <param name="file">The name of the file. If this is a path, will be truncated with <see cref="Path.GetFileName(string)"/></param>
        /// <returns>An object that associates a given file with a with a unique <see cref="Guid"/></returns>
        IFile OpenFile(string file);

        /// <summary>
        /// Copies a file from an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// Do not use this method to copy from a managed <see cref="IDirectory"/>.
        /// Instead, use <see cref="CopyFrom(IFile)"/>.
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
        /// Instead, use <see cref="CopyFrom(IFile)"/>.
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
        /// Instead, use <see cref="CopyFrom(IFile)"/>.
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
        /// Instead, use <see cref="CopyFrom(IFile)"/>.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <param name="cancellation">A cancellation token that is forwarded to the underlying <see cref="Task{TResult}"/>.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation = default);

        /// <summary>
        /// Moves a file between <see cref="IDirectory"/>, updating the 
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// 
        /// The source file will cease to exist in its original <see cref="IDirectory"/>.
        ///
        /// There is no asychronous equivalent by design, since <see cref="MoveFrom(IFile, bool)"/> is intended to
        /// be faster than <see cref="CopyFromAsync(IFile, CancellationToken)"/> if the <see cref="IDirectory"/> instances
        /// are on the same file system. Otherwise, you should use <see cref="CopyFromAsync(IFile, CancellationToken)"/>,
        /// then <see cref="IFile.Delete"/> the old file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile MoveFrom(IFile source);

        /// <summary>
        /// Moves a file between <see cref="IDirectory"/>, updating the 
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// 
        /// The source file will cease to exist in its original <see cref="IDirectory"/>.
        /// 
        /// There is no asychronous equivalent by design, since <see cref="MoveFrom(IFile, bool)"/> is intended to
        /// be faster than <see cref="CopyFromAsync(IFile, bool, CancellationToken)"/> if the <see cref="IDirectory"/> instances
        /// are on the same file system. Otherwise, you should use <see cref="CopyFromAsync(IFile, bool, CancellationToken)"/>,
        /// then <see cref="IFile.Delete"/> the old file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        IFile MoveFrom(IFile source, bool overwrite);

        /// <summary>
        /// Copies a file from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(IFile source);

        /// <summary>
        /// Copies a file from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile CopyFrom(IFile source, bool overwrite);

        /// <summary>
        /// Copies a file asynchronously from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="cancellation">Cancellation token for the asynchronous task.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(IFile source, CancellationToken cancellation = default);

        /// <summary>
        /// Copies a file asynchronously from a <see cref="IFile"/> from another <see cref="IDirectory"/>, updating the
        /// manifests such that the resulting file has the same <see cref="IFile.FileGuid"/> as the source file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        /// <param name="cancellation">Cancellation token for the asynchronous task.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        Task<IFile> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancellation = default);

        /// <summary>
        /// Enumerates all direct child directories of this <see cref="IDirectory"/>.
        /// </summary>
        /// <returns>All direct children directories.</returns>
        IEnumerable<IDirectory> EnumerateDirectories();

        /// <summary>
        /// Enumerates all files in this directory,
        /// </summary>
        /// <returns>All direct children files.</returns>
        IEnumerable<IFile> EnumerateFiles();

        /// <summary>
        /// Recursively enumerates all files that are contained in this directory.
        /// 
        /// This method is usually implemented as a Breadth-first search, but no order is guaranteed.
        /// </summary>
        /// <returns>All files contained within this directory, including descendant subfolders.</returns>
        IEnumerable<IFile> EnumerateFilesRecursive();

        /// <summary>
        /// Whether or not this directory contains a file in its manifest. If provided a
        /// full path, this will truncate the path using <see cref="Path.GetFileName(string)"/>
        /// </summary>
        /// <param name="file">The name of the file to check.</param>
        /// <returns>Whether or not this directory contains the given file.</returns>
        bool ContainsFile(string file);

        /// <summary>
        /// Whether or not this directory contains directory as a direct child.
        /// full path, this will truncate the path using <see cref="Path.GetDirectoryName(string)"/>
        /// </summary>
        /// <param name="directory">The name of the directory to check.</param>
        /// <returns>Whether or not this directory contains the given directory.</returns>
        bool ContainsDirectory(string directory);
    }
}
