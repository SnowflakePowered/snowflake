using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents the root of a Directory, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// 
    /// A DeletableMoveFromableDirectory allows files to be moved into this directory from other files, as well as being deleted.
    /// </summary>
    public interface IDeletableMoveFromableDirectory 
        : IDeletableDirectory
    {
        /// <summary>
        /// Moves a file between 2 <see cref="IDirectory"/> instances, updating the 
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// 
        /// The source file will cease to exist in its original <see cref="IDirectory"/>.
        ///
        /// There is no asychronous equivalent by design, since <see cref="MoveFrom(IFile, bool)"/> is intended to
        /// be faster than <see cref="IMutableDirectoryBase.CopyFromAsync(IReadOnlyFile, CancellationToken)"/> if the <see cref="IDirectory"/> instances
        /// are on the same file system. Otherwise, you should use <see cref="IMutableDirectoryBase.CopyFromAsync(IReadOnlyFile, CancellationToken)"/>,
        /// then <see cref="IFile.Delete"/> the old file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile MoveFrom(IFile source);

        /// <summary>
        /// Moves a file between <see cref="IDirectory"/>, updating the 
        /// manifests such that the resulting file has the same <see cref="IReadOnlyFile.FileGuid"/> as the source file.
        /// 
        /// The source file will cease to exist in its original <see cref="IDirectory"/>.
        /// 
        /// There is no asychronous equivalent by design, since <see cref="MoveFrom(IFile, bool)"/> is intended to
        /// be faster than <see cref="IMutableDirectoryBase.CopyFromAsync(IReadOnlyFile, bool, CancellationToken)"/> if the <see cref="IDirectory"/> instances
        /// are on the same file system. Otherwise, you should use <see cref="IMutableDirectoryBase.CopyFromAsync(IReadOnlyFile, bool, CancellationToken)"/>,
        /// then <see cref="IFile.Delete"/> the old file.
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/></param>
        IFile MoveFrom(IFile source, bool overwrite);
    }
}
