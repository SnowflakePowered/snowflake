using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Extensibility;

namespace Snowflake.Execution.Process
{
    public interface IEmulatorTaskRootDirectoryProvider
    {
        DirectoryInfo GetTaskRoot();
    }
}
