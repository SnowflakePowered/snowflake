using Snowflake.Configuration.Input;
using Snowflake.Orchestration.Saving;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Orchestration.Extensibility
{
    public abstract class GameEmulation : IAsyncDisposable, IGameEmulation
    {
        public IGame Game { get; }

        public IEnumerable<IEmulatedController> ControllerPorts { get; }

        public ISaveGame? InitialSave { get; }
        public GameEmulation(IGame game,
            IEnumerable<IEmulatedController> controllerPorts,
            ISaveGame? initialSave)
        {
            this.Game = game;
            this.ControllerPorts = controllerPorts;
            this.InitialSave = initialSave;
        }

        public abstract Task SetupEnvironment();

        public abstract Task CompileConfiguration();

        public abstract Task RestoreSaveGame();

        public abstract CancellationToken StartEmulation();

        public abstract Task StopEmulation();

        public abstract Task<ISaveGame> PersistSaveGame();

        protected abstract Task TeardownGame();

        #region IAsyncDisposable Support

        private bool IsDisposed { get; set; } = false;
        public async ValueTask DisposeAsync()
        {
            if (this.IsDisposed) return;
            this.IsDisposed = true;
            await this.StopEmulation();
            await this.PersistSaveGame();
            await this.TeardownGame();
        }
        #endregion
    }

    public abstract class GameEmulation<TConfigurationCollection> : GameEmulation
        where TConfigurationCollection : class, IConfigurationCollection<TConfigurationCollection>
    {
        public TConfigurationCollection ConfigurationProfile { get; }

        public GameEmulation(IGame game,
           TConfigurationCollection configuration,
           IEnumerable<IEmulatedController> controllerPorts,
           ISaveGame? initialSave) : base(game, controllerPorts, initialSave)
        {
            this.ConfigurationProfile = configuration;
        }
    }
}
