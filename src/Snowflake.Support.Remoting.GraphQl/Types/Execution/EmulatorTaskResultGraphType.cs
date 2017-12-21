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
            Field(t => t.EmulatorName);
        }
    }
}
