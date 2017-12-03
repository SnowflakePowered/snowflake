using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Persistence;
using System.Data.Common;

namespace Snowflake.Records.Game
{
    internal class SqliteGameLibrary : SqliteRecordLibrary<IGameRecord>, IGameLibrary
    {
        public override IMetadataLibrary MetadataLibrary { get; }
        public IFileLibrary FileLibrary { get; }
        private RecordLibraryJunction<IGameRecord, IFileRecord> FileJunction { get; }
        private readonly ISqlDatabase backingDatabase;
        public SqliteGameLibrary(ISqlDatabase database, SqliteMetadataLibrary metadataLibrary) : base(database, "games")
        {
            this.backingDatabase = database;
            this.MetadataLibrary = metadataLibrary;
            this.FileLibrary = new SqliteFileLibrary(database, metadataLibrary);
            this.FileJunction = this.CreateJunction<IFileRecord>(this.FileLibrary as SqliteFileLibrary);
        }

        public SqliteGameLibrary(ISqlDatabase database) : this(database, new SqliteMetadataLibrary(database))
        {

        }

        public override void Set(IGameRecord record)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                this.FileJunction.DeleteAllRelations(record, dbConnection);  // ensure relations do not conflict.
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES (@Guid)", record);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, path, mimetype) 
                                         VALUES (@Guid, @FilePath, @MimeType)", record.Files);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)",
                                        record.Metadata.Values.Concat(record.Files.SelectMany(m => m.Metadata.Values)));
                this.FileJunction.MakeRelation(record, record.Files, dbConnection);
            });
        }

        public override void Set(IEnumerable<IGameRecord> games)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                var gameRecords = games as IList<IGameRecord> ?? games.ToList();
                this.FileJunction.DeleteAllRelations(games.Select(g => g.Guid), dbConnection); // ensure relations do not conflict.
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES (@Guid)", gameRecords);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, path, mimetype) 
                                       VALUES (@Guid, @FilePath, @MimeType)",
                                       gameRecords.SelectMany(g => g.Files));
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                       VALUES (@Guid, @Record, @Key, @Value)",
                                       gameRecords.SelectMany(g => g.Metadata.Values
                                       .Concat(g.Files.SelectMany(m => m.Metadata.Values)))); //concatenate the files and game metadata at once
                dbConnection.Execute($@"INSERT OR REPLACE into games_files(games_uuid, files_uuid)
                                   VALUES (@parentUuid, @childUuid)", gameRecords
                                   .SelectMany(g => g.Files.Select(f => new { parentUuid = g.Guid, childUuid = f.Guid })));
            });
        }

        public override void Remove(IGameRecord record)
        {
            this.Remove(record.Guid);
        }

        public override void Remove(IEnumerable<IGameRecord> records)
        {
            this.Remove(records.Select(g => g.Guid));
        }

        public override void Remove(IEnumerable<Guid> games)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                this.FileJunction.DeleteAllRelations(games, dbConnection);
                dbConnection.Execute(@"DELETE FROM games WHERE uuid IN @games;
                                           DELETE FROM metadata WHERE record IN @games", new { games });
            });
        }

        public override IGameRecord Get(Guid game)
        {
            const string sql =
                          @"SELECT * FROM games WHERE uuid = @game;
                            SELECT * FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid = @game;
                            SELECT * FROM metadata WHERE record IN 
                                (SELECT files.uuid FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid = @game
                                 UNION ALL SELECT uuid from games WHERE uuid = @game)";
            return this.GetSingleByQuery(sql, new { game });
        }

        public override IEnumerable<IGameRecord> Get(IEnumerable<Guid> games)
        {
            const string sql = @"SELECT * from games WHERE uuid IN @games;
                                 SELECT * FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN @games;
                                 SELECT * FROM metadata WHERE record IN 
                                        (SELECT uuid from games WHERE uuid IN @games 
                                        UNION ALL SELECT files.uuid FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN @games)";
            return this.GetMultipleByQuery(sql, games);
        }

        public override void Remove(Guid game)
        {
            this.backingDatabase.Execute(dbConnection => {
                this.FileJunction.DeleteAllRelations(game, dbConnection);
                dbConnection.Execute(@"DELETE FROM games WHERE uuid = @game;
                                       DELETE FROM metadata WHERE record = @game", new { game });
                }
             );
            //because file record guids are derived from game library, they can be safely left alone
        }

        //select files.* from games_files join files on files.uuid = files_uuid and games_uuid = x'45379b93e1eb064a9bb63deda29a242d'
        public override IEnumerable<IGameRecord> SearchByMetadata(string key, string likeValue)
        {
            const string sql = @"SELECT * FROM games WHERE uuid IN 
                                (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue);

                                SELECT * FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN 
                                    (SELECT uuid FROM games WHERE uuid IN 
                                        (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue));

                                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue)
                                    UNION ALL SELECT files.uuid FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue)))";
            //this sql is gross, can we shorten this somehow?
            return this.GetMultipleByQuery(sql, new {key, likeValue = $"%{likeValue}%"});
        }

        public override IEnumerable<IGameRecord> GetByMetadata(string key, string exactValue)
        {
            const string sql = @"SELECT * FROM games WHERE uuid IN 
                                (SELECT record FROM metadata WHERE key = @key AND value = @exactValue);

                                SELECT * FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN  
                                    (SELECT uuid FROM games WHERE uuid IN 
                                        (SELECT record FROM metadata WHERE key = @key AND value = @exactValue));

                                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value = @exactValue)
                                    UNION ALL SELECT files.uuid FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN (SELECT uuid FROM games WHERE uuid IN 
                                    (SELECT record FROM metadata WHERE key = @key AND value = @exactValue)))";
            return this.GetMultipleByQuery(sql, new { key, exactValue });

        }

        public override IEnumerable<IGameRecord> GetAllRecords()
        {
            const string sql = @"SELECT * FROM games;
                                 SELECT * FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN (SELECT uuid FROM games);
                                 SELECT * FROM metadata WHERE record IN 
                                        (SELECT uuid FROM games 
                                        UNION ALL SELECT files.uuid FROM games_files JOIN files ON files.uuid = files_uuid AND games_uuid IN (SELECT uuid FROM games))";
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

                        var files = query.Read().Select(file => (
                            Game: new Guid(file.games_uuid),
                            Guid: new Guid(file.uuid),
                            Path: (string)file.path,
                            MimeType: (string)file.mimetype
                        ));

                        var metadatas = query.Read().Select(metadata => (
                            Guid: new Guid(metadata.uuid),
                            Record: new Guid(metadata.record),
                            Key: (string)metadata.key,
                            Value: (string)metadata.value
                        )).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value))?
                        .Cast<IRecordMetadata>();

                        var fileRecords = (from f in files
                                           let md = (from m in metadatas where m.Record == f.Guid select m)
                                               .ToDictionary(md => md.Key, md => md)
                                           select (game: f.Game, file: new FileRecord(f.Guid, md, f.Path, f.MimeType))).ToList();

                        return (from game in games
                                let gameFiles =
                                    (from f in fileRecords where f.game == game select f.file)
                                    .Cast<IFileRecord>().ToList()
                                let md = (from m in metadatas where m.Record == game select m)
                                    .ToDictionary(md => md.Key, md => md)
                                select new GameRecord(game, md, gameFiles)).ToList();
                    }
                    catch (DbException)
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
                        var files = query.Read().Select(f => (
                        
                            Guid: new Guid(f.uuid),
                            Path: (string)f.path,
                            MimeType: (string)f.mimetype
                       ));

                        var metadatas = query.Read().Select(metadata =>
                        (
                            Guid: new Guid(metadata.uuid),
                            Record: new Guid(metadata.record),
                            Key: (string)metadata.key,
                            Value: (string)metadata.value
                        )).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value))
                        .Cast<IRecordMetadata>();

                        var fileRecords = (from f in files
                                           let md = (from m in metadatas where m.Record == f.Guid select m)
                                               .ToDictionary(md => md.Key, md => md)
                                           select new FileRecord(f.Guid, md, f.Path, f.MimeType))
                                           .Cast<IFileRecord>().ToList();
                        var gameMetadata =
                            (from m in metadatas where m.Record == gameGuid select m)
                            .ToDictionary(m => m.Key, m => m);
                        return new GameRecord(gameGuid, gameMetadata, fileRecords);
                    }
                    catch (DbException)
                    {
                        return null;
                    }
                }
            });
        }
    }
}
