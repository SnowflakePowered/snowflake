using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallTask
{
    public class InstallTaskArguments
    {
        [PositionalArgument(0, "The package to install.")]
        public string PackagePath { get; set; }
        [NamedArgument("o", "output", "The module directory. Defaults to Snowflake application data.")]
        public string ModuleDirectory { get; set; }
        [NamedArgument("f", "noverify", "Ignores package verification.")]
        public bool NoVerify { get; set; }
    }
}