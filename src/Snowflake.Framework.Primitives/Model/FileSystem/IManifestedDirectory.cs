using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Model.FileSystem
{
    /// <summary>
    /// A directory that keeps track of contained folders by way of
    /// a manifest of files with GUID and mimetypes.
    /// 
    /// Manifested directories allow 
    /// </summary>
    public interface IManifestedDirectory : IDirectory
    {
        /// <summary>
        /// Enumerates files that have an entry in the manifest
        /// </summary>
        /// <returns></returns>
        IEnumerable<IManifestedFile> EnumerateManifestedFiles();

        /// <summary>
        /// Returns if the given file exists, and is manifested.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <returns></returns>
        bool ContainsManifestedFile(string file);

        /// <summary>
        /// Returns if the given GUID is in the manifest
        /// </summary>
        /// <param name="fileguid">The guid of the file.</param>
        /// <returns>If the GUID exists in the manifest</returns>
        bool ContainsManifestedFile(Guid fileguid);

        /// <summary>
        /// Opens or creates a file.
        /// If the given file exists, but is not manifested,
        /// adds it to the manifest with a generic mimetype
        /// of application/octet-stream.
        /// </summary>
        /// <param name="file">The name of the file</param>
        /// <returns></returns>
        new IManifestedFile OpenFile(string file);

        /// <summary>
        /// Opens the file with the given GUID. If the GUID does not
        /// exist, returns null. If the GUID exists in the manifest,
        /// but the file does not exist, the GUID is removed from the
        /// manifest.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        IManifestedFile? OpenFile(Guid guid);

        /// <summary>
        /// Sets the file with the given GUID to the
        /// given MimeType.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        IManifestedFile? SetMimeType(Guid file, string mimetype);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        new IManifestedFile? CopyFrom(FileInfo source);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        new IManifestedFile? CopyFrom(FileInfo source, bool overwrite);

        /// <summary>
        /// Copies a file from 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        IManifestedFile? CopyFrom(FileInfo source, string mimetype);


        /// <summary>
        /// Copies a file from 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        IManifestedFile? CopyFrom(FileInfo source, string mimetype, bool overwrite);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(FileInfo source);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(FileInfo source, bool overwrite);


        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(FileInfo source, bool overwrite, CancellationToken cancel);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype, bool overwrite);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(FileInfo source, string mimetype, bool overwrite, CancellationToken cancel);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        new IManifestedFile? CopyFrom(IFile source);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        new IManifestedFile? CopyFrom(IFile source, bool overwrite);

        /// <summary>
        /// Copies a file from 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        IManifestedFile? CopyFrom(IFile, string mimetype);


        /// <summary>
        /// Copies a file from 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mimetype"></param>
        /// <returns></returns>
        IManifestedFile? CopyFrom(IFile source, string mimetype, bool overwrite);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(IFile source);

        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(IFile source, bool overwrite);


        /// <summary>
        /// Copies a file from the source if it exists.
        /// Tries to infer a mimetype from the source file, 
        /// if none is available, assigns it the mimetype
        /// <pre>application/octet-stream</pre>
        /// </summary>
        /// <param name="source">The source file</param>
        /// <returns></returns>
        new Task<IManifestedFile?> CopyFromAsync(IFile source, bool overwrite, CancellationToken cancel);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype, bool overwrite);

        /// <summary>
        /// Copies a file from the source asynchronously, assigning it the
        /// given mimetype.
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="mimetype">The mimetype.</param>
        /// <returns></returns>
        Task<IManifestedFile?> CopyFromAsync(IFile source, string mimetype, bool overwrite, CancellationToken cancel);

        new IManifestedFile? MoveFrom(IFile source);

        new IManifestedFile? MoveFrom(IFile source, bool overwrite);

        IManifestedFile? MoveFrom(IFile source, bool overwrite, string mimetype);

        IManifestedFile? MoveFrom(IManifestedFile source);

        IManifestedFile? MoveFrom(IManifestedFile source, bool overwrite);
    }
}
