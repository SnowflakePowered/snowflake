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
    public abstract class GameEmulation : IDisposable
    {
        public IGame Game { get; }

        public string ConfigurationProfile { get; }

        public IList<IEmulatedController> ControllerPorts { get; }

        public GameEmulation(IGame game)
        {
            this.Game = game;
        }

        public abstract Task SetupEnvironment();

        public abstract Task CompileConfiguration();

        public abstract Task RestoreSaveGame(SaveGame targetDirectory);

        public abstract Task PersistSaveGame(IDirectory targetDirectory);

        public abstract CancellationToken RunGame();
        protected abstract void TeardownGame();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.TeardownGame();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
