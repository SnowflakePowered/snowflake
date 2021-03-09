using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a directory that is disposable.
    /// </summary>
    public interface IDisposableDirectory
        : IDirectory, IDisposable
    {
    }
}
