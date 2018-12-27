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
            this.SavesRoot = this.Root.OpenManifestedDirectory("saves");
            this.ProgramRoot = this.Root.OpenManifestedDirectory("program");
            this.MediaRoot = this.Root.OpenManifestedDirectory("media");
            this.ResourceRoot = this.Root.OpenManifestedDirectory("resource");
            this.RuntimeRoot = this.Root.OpenManifestedDirectory("runtime");
        }

        private IDirectory Root { get; }

        public IManifestedDirectory SavesRoot { get; }

        public IManifestedDirectory ProgramRoot { get; }

        public IManifestedDirectory MediaRoot { get; }

        public IManifestedDirectory MiscRoot { get; }

        public IManifestedDirectory ResourceRoot { get; }

        public IDirectory RuntimeRoot { get; }

        public IGameRecord Record { get; }

        public IEnumerable<IFileRecord> Files => throw new NotImplementedException();

        public IDirectory GetRuntimeLocation()
        {
            return this.RuntimeRoot.OpenDirectory(Guid.NewGuid().ToString());
        }

        public IManifestedDirectory GetSavesLocation(string saveType)
        {
            return this.SavesRoot.OpenManifestedDirectory(saveType);
        }
    }
}
