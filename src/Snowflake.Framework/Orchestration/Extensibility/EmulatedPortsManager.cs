using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility.Extensions;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    public sealed class EmulatedPortsManager : IEmulatedPortsManager
    {
        public EmulatedPortsManager(IEmulatedPortStore portsStore,
            IDeviceEnumerator deviceEnumerator,
            IControllerElementMappingProfileStore mappingsStore,
            IStoneProvider stoneProvider)
        {
            this.Ports = portsStore;
            this.Devices = deviceEnumerator;
            this.Mappings = mappingsStore;
            this.StoneProvider = stoneProvider;
        }

        private IEmulatedPortStore Ports { get; }
        private IDeviceEnumerator Devices { get; }
        private IControllerElementMappingProfileStore Mappings { get; }
        private IStoneProvider StoneProvider { get; }

        public IEmulatedController? GetControllerAtPort(IEmulatorOrchestrator orchestrator,
            PlatformId platform, int portIndex)
        {
            var portEntry = this.Ports.GetPort(orchestrator, platform, portIndex);
            if (portEntry == null) return null;
            var device = this.Devices.GetPortDevice(portEntry);
            if (device == null) return null;
            var instance = device.Instances.FirstOrDefault(i => i.Driver == portEntry.Driver);
            if (instance == null) return null;
            var profile = this.Mappings.GetMappings(portEntry.ProfileGuid);
            if (profile == null) return null;
            if (!this.StoneProvider.Controllers.TryGetValue(portEntry.ControllerID, out var targetLayout)) 
                return null;
            return new EmulatedController(portIndex, device, instance, targetLayout, profile);
        }
    }
}
