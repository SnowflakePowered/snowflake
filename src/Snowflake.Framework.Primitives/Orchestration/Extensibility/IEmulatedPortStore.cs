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
    public interface IEmulatedPortStore
    {
        IEmulatedPortDeviceEntry? GetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber);
        public void SetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber, ControllerId controller,
           IInputDevice device, IInputDeviceInstance instance, string inputProfile);
        void ClearPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber);
        IEnumerable<IEmulatedPortDeviceEntry> EnumeratePorts(IEmulatorOrchestrator orchestrator, PlatformId platform);
    }
}
