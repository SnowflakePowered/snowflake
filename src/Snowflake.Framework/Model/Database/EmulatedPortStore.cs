using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Database
{
    internal class EmulatedPortStore : IEmulatedPortStore
    {

        public EmulatedPortStore(string orchestratorName, EmulatedPortStoreProvider provider)
        {
            this.OrchestratorName = orchestratorName;
            this.Provider = provider;
        }

        private string OrchestratorName { get; }
        private EmulatedPortStoreProvider Provider { get; }

        public void ClearPort(PlatformId platform, int portNumber)
        {
            this.Provider.ClearPort(platform, portNumber, this.OrchestratorName);
        }

        public IEnumerable<IEmulatedPortDeviceEntry> EnumeratePorts(PlatformId platform)
        {
            return this.Provider.GetPortsForPlatform(platform, this.OrchestratorName);
        }

        public IEmulatedPortDeviceEntry? GetPort(PlatformId platform, int portNumber)
        {
            return this.Provider.GetPort(platform, portNumber, this.OrchestratorName);
        }

        public void SetPort(PlatformId platform, int portNumber, ControllerId controller, 
            IInputDevice device, IInputDeviceInstance instance, string inputProfile)
        {
            this.Provider.SetPort(platform, portNumber, controller, device, instance, inputProfile, this.OrchestratorName);
        }
    }
}
