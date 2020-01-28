using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// A global store that persists controller settings for a set of emulated ports. 
    /// </summary>
    public interface IEmulatedPortStore
    {
        /// <summary>
        /// Gets the saved <see cref="IEmulatedPortDeviceEntry"/> for the given <see cref="IEmulatorOrchestrator"/>,
        /// platform, and port number combination.
        /// </summary>
        /// <param name="orchestrator">The <see cref="IEmulatorOrchestrator"/> for the entry.</param>
        /// <param name="platform">The Stone <see cref="PlatformId"/> that specifies the platform for this set of ports.</param>
        /// <param name="portNumber">The port number.</param>
        /// <returns></returns>
        IEmulatedPortDeviceEntry? GetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber);

        /// <summary>
        /// Sets the saved <see cref="IEmulatedPortDeviceEntry"/> for the given <see cref= "IEmulatorOrchestrator" />,
        /// platform, and port number combination.
        /// 
        /// Only connected devices can be set to a port. Use <see cref="ClearPort(IEmulatorOrchestrator, PlatformId, int)"/> to
        /// "disconnect" or unassign a port entry.
        /// </summary>
        /// <param name="orchestrator">The <see cref="IEmulatorOrchestrator"/> to save this port entry for.</param>
        /// <param name="platform">The <see cref="PlatformId"/> to assign this port to.</param>
        /// <param name="portNumber">The port number to assign.</param>
        /// <param name="controller">The <see cref="ControllerId"/> of the virtual controller to assign this port to.</param>
        /// <param name="device">The real <see cref="IInputDevice"/> that this port originates from.</param>
        /// <param name="instance">The <see cref="IInputDeviceInstance"/> of the device to use for this port.</param>
        /// <param name="inputProfile">The name of the input profile to use.</param>
        public void SetPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber, ControllerId controller,
           IInputDevice device, IInputDeviceInstance instance, string inputProfile);

        /// <summary>
        /// Clears the given port for the given <see cref="IEmulatorOrchestrator"/> for the given platform.
        /// </summary>
        /// <param name="orchestrator">The <see cref="IEmulatorOrchestrator"/> to clear the port for.</param>
        /// <param name="platform">The <see cref="PlatformId"/> of the platform to clear the port for.</param>
        /// <param name="portNumber">The port number to clear.</param>
        void ClearPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portNumber);

        /// <summary>
        /// Retrieves all <see cref="IEmulatedPortDeviceEntry"/> for a given <see cref="PlatformId"/>,
        /// for a given <see cref="IEmulatorOrchestrator"/>.
        /// </summary>
        /// <param name="orchestrator">The <see cref="IEmulatorOrchestrator"/> to retrieve the set of port entries for.</param>
        /// <param name="platform">The <see cref="PlatformId"/> of the platform to retrieve the set of port entries for.</param>
        /// <returns>All <see cref="IEmulatedPortDeviceEntry"/> relevant for the given orchestrator and platform.</returns>
        IEnumerable<IEmulatedPortDeviceEntry> EnumeratePorts(IEmulatorOrchestrator orchestrator, PlatformId platform);
    }
}
