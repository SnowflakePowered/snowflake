using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Microsoft.Data.Sqlite;

namespace Snowflake.Records.Game
{
    internal class SqliteGameLibrary : IGameLibrary
    {
        public IMetadataLibrary MetadataLibrary { get; }
        public IFileLibrary FileLibrary { get; }

        private readonly SqliteDatabase backingDatabase;
        public SqliteGameLibrary(SqliteDatabase database, SqliteMetadataLibrary metadataLibrary)
        {
            this.backingDatabase = database;
            this.MetadataLibrary = metadataLibrary;
            this.FileLibrary = new SqliteFileLibrary(database, metadataLibrary);
            this.CreateDatabase();
        }

        public SqliteGameLibrary(SqliteDatabase database) : this(database, new SqliteMetadataLibrary(database))
        {

        }

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("games",
                "uuid UUID PRIMARY KEY");
        }
        public void Set(IGameRecord record)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES (@Guid)", record);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (@Guid, @Record, @FilePath, @MimeType)", record.Files);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", 
                                        record.Metadata.Values.Concat(record.Files.SelectMany(m => m.Metadata.Values)));
            });
        }

        public void Set(IEnumerable<IGameRecord> games)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                var gameRecords = games as IList<IGameRecord> ?? games.ToList();
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES (@Guid)", gameRecords);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                       VALUES (@Guid, @Record, @FilePath, @MimeType)", 
                                       gameRecords.SelectMany(g => g.Files));
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                       VALUES (@Guid, @Record, @Key, @Value)",
                                       gameRecords.SelectMany(g => g.Metadata.Values
                                       .Concat(g.Files.SelectMany(m => m.Metadata.Values)))); //concatenate the files and game metadata at once
            });
        }

        public void Remove(IGameRecord record)
        {
            this.Remove(record.Guid);
        }

        public void Remove(IEnumerable<IGameRecord> records)
        {
            this.Remove(records.Select(g => g.Guid));
        }

        public void Remove(IEnumerable<Guid> games)
        {
            this.backingDatabase.Execute(@"DELETE FROM games WHERE uuid IN @games;
                                           DELETE FROM metadata WHERE record IN @games", new { games });
        }

        public IGameRecord Get(Guid game)
        {
            const string sql =
                          @"SELECT * FROM games WHERE uuid = @game;
                            SELECT * FROM files WHERE game = @game;
                            SELECT * FROM metadata WHERE record IN 
                                (SELECT uuid FROM files WHERE game = @game
                                 UNION ALL SELECT uuid from games WHERE uuid = @game)";
            return this.GetSingleByQuery(sql, new {game});
        }

        public IEnumerable<IGameRecord> Get(IEnumerable<Guid> games)
        {
            const string sql = @"SELECT * from games WHERE uuid IN @games;
                                 SELECT * FROM files WHERE game IN @games;
                                 SELECT * FROM metadata WHERE record IN 
                                        (SELECT uuid from games WHERE uuid IN @games 
                                        UNION ALL SELECT uuid FROM files WHERE game IN @games)";
            return this.GetMultipleByQuery(sql, games);
        }

        public void Remove(Guid game)
        {
            this.backingDatabase.Execute(@"DELETE FROM games WHERE uuid = @game;
                                           DELETE FROM metadata WHERE record = @game", new { game });
            //because file record guids are derived from game library, they can be safely left alone
        }

        public IEnumerable<IGameRecord> SearchByMetadata(string key, string likeValue)
        {
            const string sql = @"SELECT * FROM games WHERE uuid IN 
                                (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue);

                                SELECT * FROM files WHERE game IN 
                                    (SELECT uuid FROM games WHERE uuid IN 
                                        (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue));

                                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue)
                                    UNION ALL SELECT uuid FROM files WHERE game IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue)))";
            //this sql is gross, can we shorten this somehow?
            return this.GetMultipleByQuery(sql, new {key, likeValue = $"%{likeValue}%"});
        }

        public IEnumerable<IGameRecord> GetByMetadata(string key, string exactValue)
        {
            const string sql = @"SELECT * FROM games WHERE uuid IN 
                                (SELECT record FROM metadata WHERE key = @key AND value = @exactValue);

                                SELECT * FROM files WHERE game IN 
                                    (SELECT uuid FROM games WHERE uuid IN 
                                        (SELECT record FROM metadata WHERE key = @key AND value = @exactValue));

                                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value = @exactValue)
                                    UNION ALL SELECT uuid FROM files WHERE game IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value = @exactValue)))";
            return this.GetMultipleByQuery(sql, new { key, exactValue });

        }

        public IEnumerable<IGameRecord> GetAllRecords()
        {
            const string sql = @"SELECT * FROM games;
                                 SELECT * FROM files WHERE game IN (SELECT uuid FROM games);
                                 SELECT * FROM metadata WHERE record IN 
                                        (SELECT uuid FROM games 
                                        UNION ALL SELECT uuid FROM files WHERE game IN (SELECT uuid FROM games))";
            return this.GetMultipleByQuery(sql, null);
        }

        public IEnumerable<IGameRecord> GetGamesByTitle(string nameSearch)
        {
            return this.SearchByMetadata(GameMetadataKeys.Title, nameSearch);
        }

        public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        {
            return this.GetByMetadata(GameMetadataKeys.Platform, platformId);
        }


        /// <summary>
        /// Gets multiple game records according to the query
        /// </summary>
        /// <param name="sql">3 SQL queries together, 
        /// the first gets game records, 
        /// the second gets file records,
        /// and the third gets corresponding metadata records for both file and games.</param>
        /// <param name="param">The parameters</param>
        /// <returns>A list of file records</returns>
        private IEnumerable<IGameRecord> GetMultipleByQuery(string sql, object param)
        {
            return this.backingDatabase.Query<IEnumerable<IGameRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, param))
                {
                    try
                    {
                        var games = query.Read().Select(game => new Guid(game.uuid));

                        var files = query.Read().Select(file => new
                        {
                            Guid = new Guid(file.uuid),
                            Game = new Guid(file.game),
                            Path = file.path,
                            MimeType = file.mimetype
                        });

                        var metadata = query.Read<RecordMetadata>();
                        var fileRecords = (from f in files
                                           let md = (from m in metadata where m.Record == f.Guid select m)
                                               .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                           select new FileRecord(f.Game, md, f.Path, f.MimeType)).ToList();
                        return (from game in games
                                let gameFiles =
                                    (from f in fileRecords where f.Record == game select f as IFileRecord).ToList()
                                let md = (from m in metadata where m.Record == game select m)
                                    .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                select new GameRecord(game, md, gameFiles)).ToList();
                    }
                    catch (SqliteException)
                    {
                        return new List<IGameRecord>();
                    }

                }
            });
        }

        /// <summary>
        /// Gets a single game record according to the query
        /// </summary>
        /// <param name="sql">3 SQL queries together, 
        /// the first gets game records, 
        /// the second gets file records,
        /// and the third gets corresponding metadata records for both file and games.</param>
        /// <param name="param">The parameters</param>
        /// <returns>A single game record</returns>
        private IGameRecord GetSingleByQuery(string sql, object param)
        {
            return this.backingDatabase.Query<IGameRecord>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, param))
                {
                    try
                    {
                        dynamic _game = query.ReadFirstOrDefault();
                        if (_game == null) return null;
                        Guid gameGuid = new Guid(_game.uuid);
                        var files = query.Read().Select(f => new
                        {
                            Guid = new Guid(f.uuid),
                            Game = new Guid(f.game),
                            Path = f.path,
                            MimeType = f.mimetype
                        });

                        var metadata = query.Read<RecordMetadata>();
                        var fileRecords = (from f in files
                                           let md = (from m in metadata where m.Record == f.Guid select m)
                                               .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                           select new FileRecord(f.Game, md, f.Path, f.MimeType) as IFileRecord).ToList();
                        var gameMetadata =
                            (from m in metadata where m.Guid == gameGuid select m).ToDictionary(m => m.Key, m => m as IRecordMetadata);
                        return new GameRecord(gameGuid, gameMetadata, fileRecords);
                    }
                    catch (SqliteException)
                    {
                        return null;
                    }
                }
            });
        }
    }
}
