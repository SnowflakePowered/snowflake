using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Tasks.HelpTask
{
    public sealed class HelpTaskArguments
    {
        [PositionalArgument(0, "The task to assist with.")]
        public string Task { get; set; }
    }
}
