using Snowflake.Model.Game;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Helper service to retrieve an <see cref="IEmulatedController"/> from the <see cref="IEmulatedPortDeviceEntry"/>
    /// of a given <see cref="IEmulatorOrchestrator"/>.
    /// </summary>
    public interface IEmulatedPortsManager
    {
        /// <summary>
        /// Retrieves an <see cref="IEmulatedController"/> instance for the given <see cref="IEmulatorOrchestrator"/>,
        /// platform, and port index, if connected. Returns null otherwise.
        /// </summary>
        /// <param name="orchestrator">The <see cref="IEmulatorOrchestrator"/> to build a <see cref="IEmulatedController"/> for.</param>
        /// <param name="platform">The <see cref="PlatformId"/> of the platform to retrieve </param>
        /// <param name="portIndex">The port index </param>
        /// <returns>The <see cref="IEmulatedController"/> for the given port index, if the <see cref="IEmulatedPortDeviceEntry"/>
        /// is connected.</returns>
        IEmulatedController? GetControllerAtPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portIndex);
    }
}
