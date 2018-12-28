using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game
{
    public interface IGame 
    {
        IDirectory SavesRoot { get; }
        IDirectory ProgramRoot { get; }
        IDirectory MediaRoot { get; }
        IDirectory MiscRoot { get; }
        IDirectory ResourceRoot { get; }

        IDirectory RuntimeRoot { get; }

        IDirectory GetSavesLocation(string saveType);
        IDirectory GetRuntimeLocation();

        IGameRecord Record { get; }

        IEnumerable<IFileRecord> Files { get; }

    }
}
