using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Services;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class OrchestrationQueryBuilder
    {
        public IPluginCollection<IEmulatorOrchestrator> Orchestrators { get; }
        public IStoneProvider Stone { get; }
        public IGameLibrary GameLibrary { get; }
        public IEmulatedPortsManager Ports { get; }
        private ConcurrentDictionary<Guid, IGameEmulation> GameEmulationCache { get; }

        public OrchestrationQueryBuilder(IPluginCollection<IEmulatorOrchestrator> orchestrators,
            IStoneProvider stone,
            IGameLibrary library,
            IEmulatedPortsManager ports)
        {
            this.Orchestrators = orchestrators;
            this.Stone = stone;
            this.GameLibrary = library;
            this.Ports = ports;
        }
      
        public async Task<Guid> PrepareGameEmulation(Guid emulationHandle)
        {
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            await emulation.RestoreSaveGame();
            await emulation.SetupEnvironment();
            await emulation.CompileConfiguration();
            return emulationHandle;
        }

        public Guid StartGameEmulation(Guid emulationHandle)
        {
            // todo save cancellation handle.
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            var cancel = emulation.StartEmulation();
            cancel.Register(async () => await emulation.DisposeAsync());
            return emulationHandle;
        }

        public Guid StopGameEmulation(Guid emulationHandle)
        {
            // todo save cancellation handle.
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            emulation.StopEmulation();
            return emulationHandle;
        }
    }
}
