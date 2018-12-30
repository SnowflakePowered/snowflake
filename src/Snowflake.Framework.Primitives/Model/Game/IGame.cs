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
        IGameRecord Record { get; }

        TExtension? GetExtension<TExtension>() where TExtension : class, IGameExtension;
    }
}
