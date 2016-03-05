using DapperExtensions.Mapper;
using Snowflake.Game;

namespace Snowflake.Information.Database
{
    /// <summary>
    /// Maps a concrete GameInfo to the games table
    /// </summary>
    internal class SqliteGameDatabaseMapper : ClassMapper<GameInfo>
    {
        public SqliteGameDatabaseMapper()
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
    internal class SqliteIGameDatabaseMapper : ClassMapper<IGameInfo>
    {
        public SqliteIGameDatabaseMapper()
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
