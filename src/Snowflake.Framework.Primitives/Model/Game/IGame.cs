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
        IManifestedDirectory SavesRoot { get; }
        IManifestedDirectory ProgramRoot { get; }
        IManifestedDirectory MediaRoot { get; }
        IManifestedDirectory MiscRoot { get; }
        IManifestedDirectory ResourceRoot { get; }

        IDirectory RuntimeRoot { get; }

        IManifestedDirectory GetSavesLocation(string saveType);
        IDirectory GetRuntimeLocation();

        IGameRecord Record { get; }
        IEnumerable<IFileRecord> Files { get; }

    }
}
