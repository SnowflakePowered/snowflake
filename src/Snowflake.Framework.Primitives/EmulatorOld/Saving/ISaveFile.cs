using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.EmulatorOld.Saving
{
    public interface ISaveFile
    {
        IGameRecord GameRecord { get; }
        IFileRecord SaveFileRecord { get; }

    }
}
