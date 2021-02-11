using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    public static class FileSystemInfoExtensions
    {
        internal static Emet.FileSystems.DirectoryEntry AsDirEntry(this FileSystemInfo @this)
        {
            return new Emet.FileSystems.DirectoryEntry(@this.FullName, Emet.FileSystems.FileSystem.FollowSymbolicLinks.Always);
        }

        internal static bool IsSymbolicLink(this FileSystemInfo @this)
        {
            return @this.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        /// <summary>
        /// Gets a value indicating whether the specified file exists.
        /// Unlike <see cref="FileInfo.Exists"/>, this method follows symbolic links.
        /// </summary>
        /// <param name="this">The <see cref="FileInfo"/> representing the current file.</param>
        /// <returns><see langword="true"/> if the file exists on disk and is accessible, <see langword="false"/> otherwise.</returns>
        internal static bool Exists(this FileInfo @this)
        {
            return @this.AsDirEntry().FileType == Emet.FileSystems.FileType.File;
        }

        /// <summary>
        /// Gets a value indicating whether the specified directory exists.
        /// Unlike <see cref="DirectoryInfo.Exists"/>, this method follows symbolic links.
        /// </summary>
        /// <param name="this">The <see cref="DirectoryInfo"/> representing the current file.</param>
        /// <returns><see langword="true"/> if the directory exists on disk and is accessible, <see langword="false"/> otherwise.</returns>
        internal static bool Exists(this DirectoryInfo @this)
        {
            return @this.AsDirEntry().FileType == Emet.FileSystems.FileType.Directory;
        }

        /// <summary>
        /// Gets the size, in bytes, of the current file.
        /// Unlike <see cref="FileInfo.Length"/>, this method follows symbolic links.
        /// </summary>
        /// <param name="this">The <see cref="FileInfo"/> representing the current file.</param>
        /// <returns>The size of the file in bytes, or -1 if the file does not exist or is a directory.</returns>
        /// <exception cref="IOException"></exception>
        public static long Length(this FileInfo @this)
        {
            var dirEntry = @this.AsDirEntry();
            return dirEntry.FileType == Emet.FileSystems.FileType.File ? 
                dirEntry.FileSize : -1;
        }
    }
}
