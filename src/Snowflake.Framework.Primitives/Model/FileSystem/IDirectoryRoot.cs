using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.FileSystem
{
    public interface IDirectoryRoot
    {
        IDirectory Root { get; }
        string ResolveRealPath(string relativePath);
    }
}
