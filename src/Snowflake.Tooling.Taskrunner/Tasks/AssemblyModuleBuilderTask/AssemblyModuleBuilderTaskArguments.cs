using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask
{
    public class AssemblyModuleBuilderTaskArguments
    {
        [NamedArgument("o","output", "The directory to output the built module. Defaults to ..\\bin\\module.")]
        public string OutputDirectory { get; set; }

        [NamedArgument("s", "source", "The directory where the module sources are located. Defaults to the current working directory.")]
        public string SourceDirectory { get; set; }

        [NamedArgument("a", "arguments", "The arguments to pass to MSBuild.")]
        public string MsbuildArgs { get; set; } = "";
    }
}
