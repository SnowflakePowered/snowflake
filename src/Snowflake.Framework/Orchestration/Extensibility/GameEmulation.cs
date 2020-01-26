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

namespace Snowflake.Orchestration.Extensibility
{
    public abstract class GameEmulation : IAsyncDisposable, IGameEmulation
    {
        public IGame Game { get; }

        public string ConfigurationProfile { get; }

        public IList<IEmulatedController> ControllerPorts { get; }

        public GameEmulation(IGame game, 
            string configurationProfile,
            IList<IEmulatedController> controllerPorts)
        {
            this.Game = game;
            this.ConfigurationProfile = configurationProfile;
            this.ControllerPorts = controllerPorts;
        }

        public abstract Task SetupEnvironment();

        public abstract Task CompileConfiguration();

        public abstract Task RestoreSaveGame(ISaveGame targetDirectory);

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
}
