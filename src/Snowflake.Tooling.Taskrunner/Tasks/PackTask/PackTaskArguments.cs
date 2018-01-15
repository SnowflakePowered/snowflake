using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    public class PackTaskArguments
    {
        [PositionalArgument(0, "The module to pack.")]
        public string ModuleDirectory { get; set; }

        [NamedArgument("o", "output", "The name of the output file.")]
        public string OutputFile { get; set; }

        [NamedArgument("a", "notreeshaking", "Disable dependency tree shaking for assembly modules.")]
        public bool NoTreeShaking { get; set; }
    }
}
