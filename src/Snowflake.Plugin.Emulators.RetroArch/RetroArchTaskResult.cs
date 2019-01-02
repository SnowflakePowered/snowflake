using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;
using Snowflake.Execution.Saving;

namespace Snowflake.Adapters
{
    public class RetroArchTaskResult : EmulatorTaskResult
    {
        private IEmulatorTaskRoot TaskRoot { get; }
        private ISaveLocation SaveLocation { get; }

        public RetroArchTaskResult(IEmulatorTaskRoot taskRoot, ISaveLocation location)
            : base("retroarch")
        {
            this.TaskRoot = taskRoot;
            this.SaveLocation = location;
        }

        public override void Closed()
        {
            this.SaveLocation.PersistFrom(this.TaskRoot.SaveDirectory);
            this.TaskRoot.TaskRoot.Delete(true);
        }
    }
}
