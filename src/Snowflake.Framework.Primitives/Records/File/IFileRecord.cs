using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Records.File
{
    /// <summary>
    /// Represents a file on disk.
    /// </summary>
    public interface IFileRecord : IRecord
    {
        /// <summary>
        /// The mimetype of the file
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// The file path of the file
        /// </summary>
        string FilePath { get; }
    }
}
