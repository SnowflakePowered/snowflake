using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Extensibility
{
    public abstract class GameEmulation
    {
        public IGame Game { get; }

        public Guid Guid { get; }

        public GameEmulation(IGame game, Guid guid)
        {
            this.Game = game;
            this.Guid = guid;
        }

        public abstract IAsyncEnumerable<TaskResult<IFile>> SetupEnvironment();
        public abstract IAsyncEnumerable<TaskResult<IFile>> SetupGame();
    }
}
