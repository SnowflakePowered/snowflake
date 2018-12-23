using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game; 

namespace Snowflake.Model.Records
{
    public interface IGame : IRecord
    {
        IDirectory SavesRoot { get; }
        IDirectory ProgramRoot { get; }
        IDirectory ImageRoot { get; }
        IDirectory MiscRoot { get; }
        IDirectory DatabaseRoot { get; }
        IDirectory RuntimeRoot { get; }

        IDirectory GetSavesLocation(string saveType);
        IDirectory GetRuntimeLocation();

        IGameRecord Record { get; }
        IEnumerable<IFileRecord> Files { get; }

    }
}
