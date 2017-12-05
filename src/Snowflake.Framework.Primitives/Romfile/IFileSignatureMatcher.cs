using System.Collections.Generic;
using System.IO;

namespace Snowflake.Romfile
{
    /// <summary>
    /// An engine that looks up file signature matches
    /// </summary>
    public interface IFileSignatureMatcher
    {
        /// <summary>
        /// Gets possible mimetypes given a file extension
        /// </summary>
        /// <param name="fileExtension">The file extension, including the dot, to lookup</param>
        /// <returns>Possible sotne mimetypes</returns>
        IEnumerable<string> GetPossibleMimetypes(string fileExtension);

        /// <summary>
        /// Registers a file signature to the matcher
        /// </summary>
        /// <param name="mimetype">The mimetype the matcher matches for</param>
        /// <param name="fileSignature">The file signature to register</param>
        void RegisterFileSignature(string mimetype, IFileSignature fileSignature);

        /// <summary>
        /// Gets rom information using registered file signatures
        /// </summary>
        /// <param name="fileName">The file name of the ROM</param>
        /// <param name="fileContents">The contents of the ROM</param>
        /// <returns>Information from the ROM</returns>
        IRomFileInfo GetInfo(string fileName, Stream fileContents);
    }
}