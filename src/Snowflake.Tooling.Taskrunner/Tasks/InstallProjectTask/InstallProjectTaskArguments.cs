using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallProjectTask
{
    public class InstallProjectTaskArguments
    {
        [NamedArgument("o", "output", "The module directory. Defaults to Snowflake application data.")]
        public string ModuleDirectory { get; set; }

        [NamedArgument("a", "notreeshaking", "Disable dependency tree shaking for assembly modules.")]
        public bool NoTreeShaking { get; set; }
    }
}
