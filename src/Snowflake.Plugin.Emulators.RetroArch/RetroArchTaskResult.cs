using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;

namespace Snowflake.Adapters
{
    public class RetroArchTaskResult : EmulatorTaskResult
    {
        private IEmulatorTaskRoot TaskRoot { get; }
        public RetroArchTaskResult(IEmulatorTaskRoot taskRoot)
            : base("retroarch")
        {
            this.TaskRoot = taskRoot;
        }

        public override void Closed()
        {
            this.TaskRoot.TaskRoot.Delete(true);
        }
    }
}
