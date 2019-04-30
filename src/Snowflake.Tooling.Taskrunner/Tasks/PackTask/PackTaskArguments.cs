using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    public class PackTaskArguments
    {
        [PositionalArgument(0, "The module to pack. If no module directory is detected, and the current directory is a " +
                               "project directory, will default to the default build location of the assembly module.")]
        public string ModuleDirectory { get; set; }

        [NamedArgument("o", "output", "The name of the output file.")]
        public string OutputFile { get; set; }

        [NamedArgument("a", "notreeshaking", "Disable dependency tree shaking for assembly modules.")]
        public bool NoTreeShaking { get; set; }
    }
}
