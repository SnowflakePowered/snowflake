using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Execution.Extensibility
{
    public interface IEmulatorTaskRunner
    {
        Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task);
    }
}
