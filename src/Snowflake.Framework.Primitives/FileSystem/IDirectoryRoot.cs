using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Filesystem
{
    public interface IDirectoryRoot
    {
        IDirectory Root { get; }
        string ResolveRealPath(string relativePath);
    }
}
