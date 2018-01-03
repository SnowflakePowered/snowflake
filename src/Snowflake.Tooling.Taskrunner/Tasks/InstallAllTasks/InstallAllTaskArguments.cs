using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallAllTask
{
    public class InstallAllTaskArguments
    {
        [NamedArgument("d", "packagedirectory", "The directory where packages are located. Defaults to current working directory.")]
        public string PackageDirectory { get; set; }
        [NamedArgument("o", "output", "The module directory. Defaults to Snowflake application data.")]
        public string ModuleDirectory { get; set; }
        [NamedArgument("f", "noverify", "Ignores package verification.")]
        public bool NoVerify { get; set; }
    }
}
