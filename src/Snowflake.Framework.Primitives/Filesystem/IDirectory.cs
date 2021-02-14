using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents the root of a Directory that can not be deleted.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IDirectory 
        : IMutableDirectoryBase,
          IFileOpeningDirectoryBase<IFile>,
          IDirectoryOpeningDirectoryBase<IDeletableDirectory>,
          IReopenableDirectoryBase<IReadOnlyDirectory>,
          IReopenableDirectoryBase<IMoveFromableDirectory>,
          IEnumerableDirectoryBase<IDeletableDirectory, IFile>
    {
        /// <summary>
        /// <para>
        /// Creates a symbolic link to an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>.
        /// 
        /// You should not link to another <see cref="IFile"/>. Instead, use <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/>.
        /// </para>
        /// <para>
        /// Links are transparent, i.e. <see cref="IFile.OpenStream()"/> will open a stream to the linked file, and
        /// <see cref="IReadOnlyFile.UnsafeGetFilePath"/> will return the path of the linked file.
        /// <see cref="IDirectory"/> methods like <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/> and 
        /// <see cref="IMoveFromableDirectoryBase.MoveFrom(IFile)"/> work as 
        /// expected.
        /// </para>
        /// <para>
        /// The underlying file that a link points to can only be modified through the stream.
        /// Calling <see cref="IFile.Delete"/> or <see cref="IFile.Rename(string)"/> will only rename
        /// or delete the link. If the file the link points to does not exist, 
        /// calling <see cref="IFile.OpenStream()"/> will throw <see cref="FileNotFoundException"/>.
        /// </para>
        /// <para>
        /// Links are very powerful and allow a way for plugins to escape the filesystem jail that an <see cref="IDeletableDirectory"/>
        /// creates. Because they present the potential to modify files outside of the allocated directory, be very careful
        /// when using them. Prefer read-only access by using <see cref="IFile.AsReadOnly"/> whenever possible.
        /// </para>
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile LinkFrom(FileInfo source);

        /// <summary>
        /// <para>
        /// Creates a link to an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>. Links are akin to shortcuts more than symbolic links,
        /// being represented as a text file with a real file path to another file on the file system.
        ///  Do not ever link to another <see cref="IFile"/>. Instead, use <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/>.
        /// </para>
        /// <para>
        /// The underlying file that a link points to can only be modified through the stream.
        /// Calling <see cref="IFile.Delete"/> or <see cref="IFile.Rename(string)"/> will only rename
        /// or delete the link. If the file the link points to does not exist, 
        /// calling <see cref="IFile.OpenStream()"/> will throw <see cref="FileNotFoundException"/>.
        /// </para>
        /// <para>
        /// Links are very powerful and allow a way for plugins to escape the filesystem jail that an <see cref="IDirectory"/>
        /// creates. Because they present the potential to modify files outside of the allocated directory, be very careful
        /// when using them. Prefer read-only access by using <see cref="IFile.AsReadOnly"/> whenever possible.
        /// </para>
        /// </summary>
        /// <exception cref="IOException">If a file with the same name exists in the target destination and <paramref name="overwrite"/> is false.</exception>
        /// <exception cref="FileNotFoundException">If the source file can not be found.</exception>
        /// <param name="source">The <see cref="FileInfo"/></param>
        /// <param name="overwrite">Overwrite the file if it already exists in this <see cref="IDirectory"/>.
        /// The new link will inherit the GUID of the previously existing file.</param>
        /// <returns>The <see cref="IFile"/> that describes the file in the current <see cref="IDirectory"/>.</returns>
        IFile LinkFrom(FileInfo source, bool overwrite);
    }
}
