using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.FileSystem
{
    public interface IManifestedFile : IFile
    {
        Guid FileGuid { get; }

        /// <summary>
        /// The mimetype for this file
        /// 
        /// If no mimetype has been set, returns
        /// <pre>application/octet-stream</pre> by default.
        /// </summary>
        string MimeType { get; }
    }
}
