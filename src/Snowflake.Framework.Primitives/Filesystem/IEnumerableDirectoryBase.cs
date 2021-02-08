using System.Collections.Generic;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Base interface for an enumerable directory.
    /// 
    /// The most basic directory type is <see cref="IDirectory"/>
    /// </summary>
    /// <typeparam name="TDirectoryEnumerableAs">The type of directory this directory will enumerate as.</typeparam>
    /// <typeparam name="TFileEnumerableAs">The type of files this directory will enumerate as.</typeparam>
    public interface IEnumerableDirectoryBase<TDirectoryEnumerableAs, TFileEnumerableAs>
    {
        /// <summary>
        /// Enumerates all direct child directories of this directory.
        /// </summary>
        /// <returns>All direct children directories.</returns>
        IEnumerable<TDirectoryEnumerableAs> EnumerateDirectories();

        /// <summary>
        /// Enumerates all files in this directory,
        /// </summary>
        /// <returns>All direct children files.</returns>
        IEnumerable<TFileEnumerableAs> EnumerateFiles();

        /// <summary>
        /// Recursively enumerates all files that are contained in this directory.
        /// 
        /// This method is usually implemented as a Breadth-first search, but no order is guaranteed.
        /// </summary>
        /// <returns>All files contained within this directory, including descendant subfolders.</returns>
        IEnumerable<TFileEnumerableAs> EnumerateFilesRecursive();
    }
}
