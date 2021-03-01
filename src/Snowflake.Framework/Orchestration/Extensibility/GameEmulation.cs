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
using Snowflake.Configuration.Internal;

namespace Snowflake.Orchestration.Extensibility
{
    public abstract class GameEmulation : IAsyncDisposable, IGameEmulation
    {
        public IGame Game { get; }

        public IEnumerable<IEmulatedController> ControllerPorts { get; }

        public ISaveProfile SaveProfile { get; }
        public GameEmulation(IGame game,
            IEnumerable<IEmulatedController> controllerPorts,
            ISaveProfile saveProfile)
        {
            this.Game = game;
            this.ControllerPorts = controllerPorts;
            this.SaveProfile = saveProfile;
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

        public GameEmulationState EmulationState { get; protected set; } = GameEmulationState.RequiresSetupEnvironment;

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
        where TConfigurationCollection : class, IConfigurationCollectionTemplate
    {
        public IConfigurationCollection<TConfigurationCollection> ConfigurationProfile { get; }

        public GameEmulation(IGame game,
           IConfigurationCollection<TConfigurationCollection> configuration,
           IEnumerable<IEmulatedController> controllerPorts,
           ISaveProfile saveProfile) : base(game, controllerPorts, saveProfile)
        {
            this.ConfigurationProfile = configuration;
        }
    }
}
