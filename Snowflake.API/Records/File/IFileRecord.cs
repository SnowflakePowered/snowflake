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
    /// <br/>
    /// Similar to a <see cref="IRecordMetadata"/>, the GUID of a file depends on the path, 
    /// and the guid of the game record. A file with the same path and game record GUID will
    /// have the same GUID.
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

        /// <summary>
        /// The game record attached to this Guid.
        /// </summary>
        Guid Record { get; }
    }
}
