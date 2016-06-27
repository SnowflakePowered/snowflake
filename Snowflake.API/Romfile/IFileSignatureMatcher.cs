using System.Collections.Generic;
using System.IO;

namespace Snowflake.Romfile
{
    /// <summary>
    /// An engine that looks up file signature matches
    /// </summary>
    public interface IFileSignatureMatcher
    {
        IEnumerable<string> GetPossibleMimetypes(string fileExtension);
        void RegisterFileSignature(string mimetype, IFileSignature fileSignature);
        IRomFileInfo GetInfo(string fileName, Stream fileContents);
    }
}