using Snowflake.Model.Game;

namespace Snowflake.Orchestration.Extensibility
{
    public interface IEmulatedPortsManager
    {
        IEmulatedController? GetControllerAtPort(IEmulatorOrchestrator orchestrator, PlatformId platform, int portIndex);
    }
}