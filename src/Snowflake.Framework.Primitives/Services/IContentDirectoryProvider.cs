using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Services
{
    public interface IContentDirectoryProvider
    {
        DirectoryInfo ApplicationData { get; }
    }
}
