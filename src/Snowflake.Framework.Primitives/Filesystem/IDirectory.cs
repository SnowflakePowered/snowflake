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
       
    }
}
