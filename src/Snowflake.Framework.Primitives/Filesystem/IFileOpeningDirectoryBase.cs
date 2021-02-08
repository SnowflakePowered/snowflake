using System;
using System.IO;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Base interface for a directory.
    /// 
    /// Never refer to this interface directly. The most basic directory type is <see cref="IDirectory"/>
    /// </summary>
    /// <typeparam name="TChildFile">The type of file this directory will open</typeparam>
    public interface IFileOpeningDirectoryBase<TChildFile>
    {
        /// <summary>
        /// Opens or creates a file, adding it to the manifest and assigning it a
        /// unique <see cref="Guid"/>.
        /// 
        /// Unlike <see cref="IDirectoryOpeningDirectoryBase{T}.OpenDirectory(string)"/>, you can not use the path separator to
        /// open a nested file. Paths will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// Instead, use <see cref="IDirectoryOpeningDirectoryBase{T}.OpenDirectory(string)"/> to open the directory of the desired file,
        /// then call <see cref="OpenFile(string)"/> on the returned instance.
        /// 
        /// Keep in mind this does not actually create a file on the underlying file system. 
        /// However, you can use <see cref="IFile.OpenStream()"/> and friends to create the file
        /// if it does not yet exist.
        /// </summary>
        /// <param name="file">The name of the file. If this is a path, will be truncated with <see cref="Path.GetFileName(string)"/></param>
        /// <returns>An object that associates a given file with a with a unique <see cref="Guid"/></returns>
        TChildFile OpenFile(string file);
    }
}
