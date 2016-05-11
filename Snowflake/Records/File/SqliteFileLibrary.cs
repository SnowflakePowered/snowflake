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
        public IMetadataStore MetadataStore { get; }

        private readonly SqliteDatabase backingDatabase;
        public SqliteFileLibrary(SqliteDatabase database)
        {
            this.backingDatabase = database;
            this.MetadataStore = new SqliteMetadataStore(database);
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

        public void Add(IFileRecord record)
        {
            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (
                                          @Guid,
                                          @GameRecord,
                                          @FilePath,
                                          @MimeType)", record);
            foreach (var metadata in record.Metadata.Values)
            {
                this.MetadataStore.Set(metadata); //todo optimize this
            }
        }

        public void Remove(IFileRecord record)
        {
            this.Remove(record.Guid);
        }

        public IFileRecord Get(Guid guid)
        {
            return this.GetFile(guid);
        }

        public void Remove(Guid guid)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @guid", new { guid });
        }

        public IEnumerable<IFileRecord> SearchByMetadata(string key, string likeValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue);
                                 SELECT * FROM metadata WHERE key = @key AND value LIKE @likeValue";
            return this.backingDatabase.Query<IEnumerable<IFileRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new { key, likeValue = $"%{likeValue}%" }))
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
                                select new FileRecord(f.Guid, f.Game, md, f.Path, f.MimeType)).ToList();
                    }
                    catch (SQLiteException)
                    {
                        return new List<FileRecord>();
                    }

                }
            });
        }

        public IEnumerable<IFileRecord> GetByMetadata(string key, string exactValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value = @exactValue);
                                 SELECT * FROM metadata WHERE key = @key AND value = @exactValue";
            return this.backingDatabase.Query<IEnumerable<IFileRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new { key , exactValue }))
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
                                select new FileRecord(f.Guid, f.Game, md, f.Path, f.MimeType)).ToList();
                    }
                    catch (SQLiteException)
                    {
                        return new List<FileRecord>();
                    }

                }
            });
        }

        public IEnumerable<IFileRecord> GetRecords()
        {
            const string sql = @"SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files);
                                SELECT * FROM files";
            return this.backingDatabase.Query<IEnumerable<IFileRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql))
                {
                    try
                    {
                        var metadata = query.Read<RecordMetadata>();
                        var files = query.Read().Select(file => new
                        {
                            Guid = new Guid(file.uuid),
                            Game = new Guid(file.game),
                            Path = file.path,
                            MimeType = file.mimetype
                        });
                        return (from f in files
                                let md = (from m in metadata where m.Record == f.Guid select m)
                                         .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                select new FileRecord(f.Guid, f.Game, md, f.Path, f.MimeType)).ToList();
                    }
                    catch (SQLiteException)
                    {
                        return new List<FileRecord>();
                    }

                }
            });
        }
        
        public IEnumerable<IFileRecord> GetFilesForGame(IGameRecord game)
        {
           return this.GetFilesForGame(game.Guid);
        }

        public IEnumerable<IFileRecord> GetFilesForGame(Guid game)
        {
            const string sql = @"SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE game = @game);
                                SELECT * FROM files WHERE game = @game";
            return this.backingDatabase.Query<IEnumerable<IFileRecord>>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new { game }))
                {
                    try
                    {
                        var metadata = query.Read<RecordMetadata>();
                        var files = query.Read().Select(file => new
                        {
                            Guid = new Guid(file.uuid),
                            Game = new Guid(file.game),
                            Path = file.path,
                            MimeType = file.mimetype
                        });
                        return (from f in files
                                let md = (from m in metadata where m.Record == f.Guid select m)
                                         .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                                select new FileRecord(f.Guid, f.Game, md, f.Path, f.MimeType)).ToList();
                    }
                    catch(SQLiteException)
                    {
                        return new List<FileRecord>();
                    }
                   
                }
            });
        }

        public IFileRecord GetFile(string filePath)
        {
            const string sql =
                @"SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE path = @filePath LIMIT 1);
                                SELECT * FROM files WHERE path = @filePath LIMIT 1";

            return this.backingDatabase.Query<IFileRecord>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new {filePath}))
                {
                    try
                    {
                        var metadata = query.Read<RecordMetadata>()?.ToDictionary(m => m.Key, m => m as IRecordMetadata);
                        dynamic _file = query.ReadFirstOrDefault();
                        if (_file == null) return null;
                        var file = new
                        {
                            Guid = new Guid(_file.uuid),
                            Game = new Guid(_file.game),
                            Path = _file.path,
                            MimeType = _file.mimetype
                        };
                        return new FileRecord(file.Guid, file.Game, metadata, file.Path, file.MimeType);
                    }
                    catch (SQLiteException)
                    {
                        return null;
                    }
                }
            });
        }

        public IFileRecord GetFile(Guid recordGuid)
        {
            const string sql =
                            @"SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid = @recordGuid LIMIT 1);
                                SELECT * FROM files WHERE uuid = @recordGuid LIMIT 1";

            return this.backingDatabase.Query<IFileRecord>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new {recordGuid}))
                {
                    try
                    {
                        var metadata = query.Read<RecordMetadata>()?.ToDictionary(m => m.Key, m => m as IRecordMetadata);
                        dynamic _file = query.ReadFirstOrDefault();
                        if (_file == null) return null;
                        var file = new
                        {
                            Guid = new Guid(_file.uuid),
                            Game = new Guid(_file.game),
                            Path = _file.path,
                            MimeType = _file.mimetype
                        };
                        return new FileRecord(file.Guid, file.Game, metadata, file.Path, file.MimeType);
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
