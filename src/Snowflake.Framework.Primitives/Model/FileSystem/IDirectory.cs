using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Snowflake.Model.FileSystem
{
    /// <summary>
    /// Represents the root of a Directory
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

        IFile OpenFile(string file);

        IEnumerable<IDirectory> EnumerateDirectories();
        IEnumerable<IFile> EnumerateFiles();

        bool ContainsFile(string file);
        bool ContainsDirectory(string directory);

        DirectoryInfo GetPath();
    }
}
