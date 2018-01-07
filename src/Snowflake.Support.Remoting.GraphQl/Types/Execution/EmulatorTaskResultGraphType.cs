using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;

namespace Snowflake.Support.Remoting.GraphQl.Types.Execution
{
    public class EmulatorTaskResultGraphType : ObjectGraphType<IEmulatorTaskResult>
    {
        public EmulatorTaskResultGraphType()
        {
            Name = "EmulatorTaskResult";
            Description = "The result of a running task.";
            Field(t => t.EmulatorName).Description("The name of the emulator executing this task.");
            Field(t => t.IsRunning).Description("Whether or not this task is currently running.");
            //Field(t => t.StartTime).Description("The time this task has been started.");
        }
    }
}
