using Snowflake.Configuration.Input;
using Snowflake.Execution.Saving;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Execution.Extensibility
{
    public abstract class GameEmulation : IDisposable
    {
        public IGame Game { get; }

        public Guid Guid { get; }

        public string ConfigurationProfile { get; }

        public IList<IEmulatedController> ControllerPorts { get; }

        public GameEmulation(IGame game, 
            Guid guid)
        {
            this.Game = game;
            this.Guid = guid;
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
