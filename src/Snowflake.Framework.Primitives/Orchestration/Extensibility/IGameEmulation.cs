using Snowflake.Model.Game;
using Snowflake.Orchestration.Saving;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Extensibility
{
    public interface IGameEmulation
    {
        string ConfigurationProfile { get; }
        IList<IEmulatedController> ControllerPorts { get; }
        IGame Game { get; }

        Task CompileConfiguration();
        ValueTask DisposeAsync();
        Task<ISaveGame> PersistSaveGame();
        Task RestoreSaveGame(ISaveGame targetDirectory);
        Task SetupEnvironment();
        CancellationToken StartEmulation();
        Task StopEmulation();
    }
}