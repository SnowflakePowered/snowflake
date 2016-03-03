using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions.Mapper;
using Newtonsoft.Json;

namespace Snowflake.Game.Database
{
    /// <summary>
    /// Maps a concrete GameInfo to the games table
    /// </summary>
    internal class GameDatabaseMapper : ClassMapper<GameInfo>
    {
        public GameDatabaseMapper()
        {
            base.Table("games");
            this.Map(game => game.UUID).Column("uuid").Key(KeyType.Assigned);
            this.Map(game => game.PlatformID).Column("platform_id");
            this.Map(game => game.FileName).Column("filename");
            this.Map(game => game.Name).Column("name");
            this.Map(game => game.CRC32).Column("crc32");
            this.Map(game => game.Metadata).Column("metadata");
        }
    }

    /// <summary>
    /// Maps an IGameInfo to the games table
    /// </summary>
    internal class IGameDatabaseMapper : ClassMapper<IGameInfo>
    {
        public IGameDatabaseMapper()
        {
            base.Table("games");
            this.Map(game => game.UUID).Column("uuid").Key(KeyType.Assigned);
            this.Map(game => game.PlatformID).Column("platform_id");
            this.Map(game => game.FileName).Column("filename");
            this.Map(game => game.Name).Column("name");
            this.Map(game => game.CRC32).Column("crc32");
            this.Map(game => game.Metadata).Column("metadata");
        }
    }
}
