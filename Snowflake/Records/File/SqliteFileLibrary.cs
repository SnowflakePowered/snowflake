using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;

namespace Snowflake.Records.File
{
    public class SqliteFileLibrary : IFileLibrary
    {
        public IMetadataLibrary MetadataLibrary { get; }

        private readonly SqliteDatabase backingDatabase;
        public SqliteFileLibrary(SqliteDatabase database)
        {
            this.backingDatabase = database;
            this.MetadataLibrary = new SqliteMetadataLibrary(database);
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("files",
                "uuid UUID PRIMARY KEY",
                "game UUID",
                "path TEXT",
                "mimetype TEXT");
        }

        public void Set(IFileRecord record)
        {
            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (
                                          @Guid,
                                          @GameRecord,
                                          @FilePath,
                                          @MimeType)", record);
            this.MetadataLibrary.Set(record.Metadata.Values); //try doing this in one round trip if possible.
        }

        public void Set(IEnumerable<IFileRecord> records)
        {
            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (
                                          @Guid,
                                          @GameRecord,
                                          @FilePath,
                                          @MimeType)", records);
            this.MetadataLibrary.Set(records.SelectMany(record => record.Metadata.Values)); 
            //since metadata are unique across records, we can do this.
        }

        public void Remove(IFileRecord record)
        {
            this.Remove(record.Guid);
        }

        public void Remove(IEnumerable<IFileRecord> records)
        {
            this.Remove(records.Select(record => record.Guid));
        }

        public void Remove(Guid guid)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @guid", new { guid });
        }

        public void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid IN @guids", guids);
        }

        public IEnumerable<IFileRecord> SearchByMetadata(string key, string likeValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue);
                                 SELECT * FROM metadata WHERE key = @key AND value LIKE @likeValue";
            return this.GetMultipleByQuery(sql, new {key, likeValue = $"%{likeValue}%"});
        }

        public IEnumerable<IFileRecord> GetByMetadata(string key, string exactValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value = @exactValue);
                                 SELECT * FROM metadata WHERE key = @key AND value = @exactValue";
            return this.GetMultipleByQuery(sql, new {key, exactValue});
        }

        public IEnumerable<IFileRecord> Get(IEnumerable<Guid> guids)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN @guids;
                                 SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid IN @guids)";
            return this.GetMultipleByQuery(sql, guids); //todo test no idea if this works
        }

        public IEnumerable<IFileRecord> GetRecords()
        {
            const string sql = @"SELECT * FROM files;
                                 SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files)";
            return this.GetMultipleByQuery(sql, null);
        }
        
        public IEnumerable<IFileRecord> GetFilesForGame(IGameRecord game)
        {
           return this.GetFilesForGame(game.Guid);
        }

        public IEnumerable<IFileRecord> GetFilesForGame(Guid game)
        {
            const string sql = @"SELECT * FROM files WHERE game = @game;
                                 SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE game = @game)";
            return this.GetMultipleByQuery(sql, new {game});
        }

        public IFileRecord Get(string filePath)
        {
            const string sql =
                @"SELECT * FROM files WHERE path = @filePath LIMIT 1;
                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE path = @filePath LIMIT 1)";
            return this.GetSingleByQuery(sql, new {filePath});
        }

        public IFileRecord Get(Guid recordGuid)
        {
            const string sql =
                            @"SELECT * FROM files WHERE uuid = @recordGuid LIMIT 1;
                              SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid = @recordGuid LIMIT 1)";

            return this.GetSingleByQuery(sql, new {recordGuid});
        }

        /// <summary>
        /// Gets multiple file records according to the query
        /// </summary>
        /// <param name="sql">2 SQL queries together, the first gets file records, and the second gets corresponding metadata records</param>
        /// <param name="param">The parameters</param>
        /// <returns>A list of file records</returns>
        private IEnumerable<IFileRecord> GetMultipleByQuery(string sql, object param)
        {
            return this.backingDatabase.Query<IEnumerable<IFileRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, param))
                {
                    try
                    {
                        var files = query.Read().Select(file => new
                        {
                            Guid = new Guid(file.uuid),
                            Game = new Guid(file.game),
                            Path = file.path,
                            MimeType = file.mimetype
                        });
                        var metadata = query.Read<RecordMetadata>();
                        return (from f in files
                                let md = (from m in metadata where m.Record == f.Guid select m)
                                         .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                select new FileRecord(f.Game, md, f.Path, f.MimeType)).ToList();
                    }
                    catch (SQLiteException)
                    {
                        return new List<FileRecord>();
                    }

                }
            });
        }

        /// <summary>
        /// Gets a single file record in accordance to query
        /// </summary>
        /// <param name="sql">2 sql queries together, the first gets file records, and the second gets corresponding metadata records</param>
        /// <param name="param">The parameters</param>
        /// <returns>A single file record</returns>
        private IFileRecord GetSingleByQuery(string sql, object param)
        {
            return this.backingDatabase.Query<IFileRecord>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, param))
                {
                    try
                    {
                        dynamic _file = query.ReadFirstOrDefault();
                        if (_file == null) return null;
                        var file = new
                        {
                            Guid = new Guid(_file.uuid),
                            Game = new Guid(_file.game),
                            Path = _file.path,
                            MimeType = _file.mimetype
                        };
                        var metadata = query.Read<RecordMetadata>()?.ToDictionary(m => m.Key, m => m as IRecordMetadata);
                        return new FileRecord(file.Game, metadata, file.Path, file.MimeType);
                    }
                    catch (SQLiteException)
                    {
                        return null;
                    }
                }
            });
        }
    }
}
