using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a readonly view over the root of a Directory, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IReadOnlyDirectory
        : IFileOpeningDirectoryBase<IReadOnlyFile>,
          IDirectoryOpeningDirectoryBase<IReadOnlyDirectory>,
          IEnumerableDirectoryBase<IReadOnlyDirectory, IReadOnlyFile>
    {
        /// <summary>
        /// Gets the name of the directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the contextual path of the directory, relative to the root of the directory provider context.
        /// For example, with paths produced by <see cref="IGameFileExtension"/>,
        /// the rooted path would be relative to <see cref="IGameFileExtension.Root"/>. Keep in mind, these paths do
        /// not have meaning outside of the context of the directory, which may or may not be known. They are also not
        /// entirely compatible with <see cref="System.IO.Path"/> APIs, and are only meaningfull within the <see cref="Snowflake.Filesystem"/>
        /// API.
        /// </summary>
        string RootedPath { get; }

        /// <summary>
        /// Opens a file if it exists.
        /// 
        /// Unlike <see cref="IDirectoryOpeningDirectoryBase{IReadOnlyDirectory}.OpenDirectory(string)"/>, you can not use the path separator to
        /// open a nested file. Paths will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// Instead, use <see cref="IDirectoryOpeningDirectoryBase{IReadOnlyDirectory}.OpenDirectory(string)"/> to open the directory of the desired file,
        /// then call <see cref="IFileOpeningDirectoryBase{IReadOnlyDirectory}.OpenFile(string)"/> on the returned instance.
        /// 
        /// Keep in mind this does not actually create a file on the underlying file system. 
        /// However, you can use <see cref="IFile.OpenStream()"/> and friends to create the file
        /// if it does not yet exist.
        /// </summary>
        /// <param name="file">The name of the file. If this is a path, will be truncated with <see cref="Path.GetFileName(string)"/></param>
        /// <param name="openIfNotExists">
        /// If true, then the handle will be created for the file if it does not exists.
        /// No actual file will be created but the filename will receive an entry in the
        /// manifest.
        /// </param>
        /// <returns>An object that associates a given file with a with a unique <see cref="Guid"/></returns>
        IReadOnlyFile OpenFile(string file, bool openIfNotExists);

        /// <summary>
        /// Whether or not this directory contains a file in its manifest. If provided a
        /// full path, this will truncate the path using <see cref="Path.GetFileName(string)"/>
        /// </summary>
        /// <param name="file">The name of the file to check.</param>
        /// <returns>Whether or not this directory contains the given file.</returns>
        bool ContainsFile(string file);

        /// <summary>
        /// Whether or not this directory contains the specified directory.
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
