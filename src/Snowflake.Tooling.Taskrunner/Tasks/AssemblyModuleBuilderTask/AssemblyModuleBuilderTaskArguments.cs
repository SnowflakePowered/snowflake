using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask
{
    public class AssemblyModuleBuilderTaskArguments
    {
        [NamedArgument("o", "output", "The directory to output the built module. Defaults to ..\\bin\\module.")]
        public string OutputDirectory { get; set; }

        [NamedArgument("s", "source",
            "The directory where the module sources are located. Defaults to the current working directory.")]
        public string SourceDirectory { get; set; }

        [NamedArgument("r", "release", "Publish a release build")]
        public bool ReleaseBuild { get; set; } = false;

        [NamedArgument("a", "arguments", "The arguments to pass to MSBuild. Use with caution.")]
        public string MsbuildArgs { get; set; } = "";
    }
}
