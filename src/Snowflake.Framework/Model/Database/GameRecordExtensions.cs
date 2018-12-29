using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Database.Contexts;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal static class GameRecordExtensions
    {
        public static GameRecordModel AsModel(this IGameRecord @this)
        {
            return new GameRecordModel()
            {
                Platform = @this.PlatformId,
                RecordID = @this.RecordId,
                RecordType = "game",
                Metadata = @this.Metadata.AsModel()
            };
        }
    }
}
