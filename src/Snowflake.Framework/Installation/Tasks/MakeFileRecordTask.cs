using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records.File;

namespace Snowflake.Installation.Tasks
{
    public sealed class MakeFileRecordTask : InstallTaskAwaitable<IFileRecord>
    {
        
        public MakeFileRecordTask(IGame game, IFile file, string mimetype)
        {
            this.Game = game;
            this.File = file;
            this.Mimetype = mimetype;
        }

        public IGame Game { get; }
        public IFile File { get; }
        public string Mimetype { get; }

        protected override string TaskName => "MakeFileRecord";

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<IFileRecord> ExecuteOnce()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.Game.WithFiles().RegisterFile(this.File, this.Mimetype);
        }
    }
}
