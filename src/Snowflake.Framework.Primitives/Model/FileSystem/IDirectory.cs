using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Snowflake.Model.FileSystem
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
        /// Opens an existing child directory with the given name.
        /// If the directory does not exist, returns null.
        /// </summary>
        /// <param name="name">The name of the existing directory</param>
        /// <returns>The directory if it exists, or null if it does not.</returns>
        IDirectory OpenDirectory(string name);

        /// <summary>
        /// Opens or creates a file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IFile OpenFile(string file);

        IFile? CopyFrom(FileInfo source);

        IFile? CopyFrom(FileInfo source, bool overwrite);

        Task<IFile?> CopyFromAsync(FileInfo source);


        Task<IFile?> CopyFromAsync(FileInfo source, bool overwrite);

        Task<IFile?> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancellation);

        IFile? MoveFrom(IFile source);

        IFile? MoveFrom(IFile source, bool overwrite);

        IFile? CopyFrom(IFile source);

        IFile? CopyFrom(IFile source, bool overwrite);

        Task<IFile?> CopyFromAsync(IFile source);

        Task<IFile?> CopyFromAsync(IFile source, bool overwrite);

        Task<IFile?> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancellation);

        IEnumerable<IDirectory> EnumerateDirectories();

        IEnumerable<IFile> EnumerateFiles();

        IEnumerable<IFile> EnumerateFilesRecursive();

        bool ContainsFile(string file);
        bool ContainsDirectory(string directory);

        DirectoryInfo GetPath();
    }
}
