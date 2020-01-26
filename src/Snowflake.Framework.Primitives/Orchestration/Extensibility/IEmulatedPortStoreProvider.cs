using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Persists controller settings for a set of emulated ports. 
    /// 
    /// </summary>
    public interface IEmulatedPortStoreProvider
    {
        IEmulatedPortStore GetPortStore(IEmulatorOrchestrator orchestrator);
    }
}
