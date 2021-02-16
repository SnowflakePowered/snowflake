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
    /// A MoveFromableDirectory allows files to be moved into this directory from other files.
    /// </summary>
    public interface IMoveFromableDirectory 
        : IDirectory, IMoveFromableDirectoryBase
    { }
}
