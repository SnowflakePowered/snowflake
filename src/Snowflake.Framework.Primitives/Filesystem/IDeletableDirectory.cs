using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a directory that can be deleted, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IDeletableDirectory : IDirectory
    {
        /// <summary>
        /// Deletes the directory, including all files and subdirectories included.
        /// 
        /// This will invalidate all instances of <see cref="IDirectory"/> pointing to
        /// this specific directory until it exists again. 
        /// </summary>       
        void Delete();

        /// <summary>
        /// Returns an undeletable version of this directory
        /// </summary>
        /// <returns></returns>
        IDirectory AsIndelible();

        /// <summary>
        /// Returns a version of this directory that can have files moved into it.
        /// </summary>
        /// <returns></returns>
        IMoveFromableDirectory AsMoveFromable();
    }
}
