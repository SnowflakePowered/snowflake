using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    public class Game : IGame
    {
        internal Game(IGameRecord record, IDirectory gameRoot)
        {
            this.Root = gameRoot;
            this.Record = record;
            this.SavesRoot = this.Root.OpenDirectory("saves");
            this.ProgramRoot = this.Root.OpenDirectory("program");
            this.MediaRoot = this.Root.OpenDirectory("media");
            this.ResourceRoot = this.Root.OpenDirectory("resource");
            this.RuntimeRoot = this.Root.OpenDirectory("runtime");
        }

        private IDirectory Root { get; }

        public IDirectory SavesRoot { get; }

        public IDirectory ProgramRoot { get; }

        public IDirectory MediaRoot { get; }

        public IDirectory MiscRoot { get; }

        public IDirectory ResourceRoot { get; }

        public IDirectory RuntimeRoot { get; }

        public IGameRecord Record { get; }

        public IEnumerable<IFileRecord> Files => throw new NotImplementedException();

        public IDirectory GetRuntimeLocation()
        {
            return this.RuntimeRoot.OpenDirectory(Guid.NewGuid().ToString());
        }

        public IDirectory GetSavesLocation(string saveType)
        {
            throw new NotImplementedException();
        }
    }
}
