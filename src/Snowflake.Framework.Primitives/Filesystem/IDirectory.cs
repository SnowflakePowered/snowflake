using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents the root of a Directory that can not be deleted, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IDirectory 
        : IMutableDirectoryBase<IDeletableDirectory>,
        IMutableDirectoryBase<IDeletableDirectory, IReadOnlyDirectory>,
        IMutableDirectoryBase<IDeletableDirectory, IMoveFromableDirectory>
    {
       
        /// <summary>
        /// Opens an existing descendant directory with the given name.
        /// If the directory does not exist, creates the directory.
        /// You can open a nested directory using '/' as the path separator, and it 
        /// will be created relative to this current directory.
        /// </summary>
        /// <param name="name">The name of the existing directory</param>
        /// <returns>The directory if it exists, or null if it does not.</returns>
        new IDeletableDirectory OpenDirectory(string name);

        /// <summary>
        /// Enumerates all direct child directories of this <see cref="IDirectory"/>.
        /// </summary>
        /// <returns>All direct children directories.</returns>
        new IEnumerable<IDeletableDirectory> EnumerateDirectories();


        /// <summary>
        /// <para>
        /// Creates a link to an unmanaged <see cref="FileInfo"/> that exists outside of
        /// a <see cref="IDirectory"/>. Links are akin to shortcuts more than symbolic links,
        /// being represented as a text file with a real file path to another file on the file system.
        ///  Do not ever link to another <see cref="IFile"/>. Instead, use <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/>.
        /// </para>
        /// <para>
        /// Links are transparent, i.e. <see cref="IFile.OpenStream()"/> will open a stream to the linked file, and
        /// <see cref="IReadOnlyFile.UnsafeGetFilePath"/> will return the path of the linked file. The path of the shortcut
        /// remains inaccessible except for the internal method <see cref="IReadOnlyFile.UnsafeGetFilePointerPath"/>. 
        /// <see cref="IDirectory"/> methods like <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/> and 
        /// <see cref="IMoveFromableDirectory.MoveFrom(IFile)"/> work as 
        /// expected.
        /// </para>
        /// <para>
        /// Links differ semantically from Files in two ways: <see cref="IReadOnlyFile.IsLink"/> is always true for links,
        /// and always false for Files, and <see cref="IFile.OpenStream()"/> on a non existing file throws <see cref="FileNotFoundException"/>
        /// instead of creating a new file. The reasoning behind throwing an exception is that links should always point to 
        /// a real file on the filesystem, and not be used as a method to escape the directory jail (although <see cref="IFile.OpenStream(FileMode, FileAccess, FileShare)"/>
        /// will work as expected. The intended action when encountering a broken link is not to create a new file, but instead to
        /// repair the link by recreating it with <see cref="LinkFrom(FileInfo)"/>.
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
        /// Links are transparent, i.e. <see cref="IFile.OpenStream()"/> will open a stream to the linked file, and
        /// <see cref="IReadOnlyFile.UnsafeGetFilePath"/> will return the path of the linked file. The path of the shortcut
        /// remains inaccessible except for the internal method <see cref="IReadOnlyFile.UnsafeGetFilePointerPath"/>. 
        /// <see cref="IDeletableDirectory"/> methods like <see cref="IMutableDirectoryBase.CopyFrom(IReadOnlyFile)"/> and <see cref="IMoveFromableDirectory.MoveFrom(IFile)"/> work as 
        /// expected.
        /// </para>
        /// <para>
        /// Links differ semantically from Files in two ways: <see cref="IReadOnlyFile.IsLink"/> is always true for links,
        /// and always false for Files, and <see cref="IFile.OpenStream()"/> on a non existing file throws <see cref="FileNotFoundException"/>
        /// instead of creating a new file. The reasoning behind throwing an exception is that links should always point to 
        /// a real file on the filesystem, and not be used as a method to escape the directory jail (although <see cref="IFile.OpenStream(FileMode, FileAccess, FileShare)"/>
        /// will work as expected. The intended action when encountering a broken link is not to create a new file, but instead to
        /// repair the link by recreating it with <see cref="LinkFrom(FileInfo, bool)"/>
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
