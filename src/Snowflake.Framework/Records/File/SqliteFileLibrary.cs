using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Snowflake.Persistence;
using System.Data.Common;

namespace Snowflake.Records.File
{
    internal class SqliteFileLibrary : IFileLibrary
    {
        public IMetadataLibrary MetadataLibrary { get; }

        private readonly ISqlDatabase backingDatabase;
        public SqliteFileLibrary(ISqlDatabase database, IMetadataLibrary metadataLibrary)
        {
            this.backingDatabase = database;
            this.MetadataLibrary = metadataLibrary;
            this.CreateDatabase();
        }

        public SqliteFileLibrary(ISqlDatabase database) : this(database, new SqliteMetadataLibrary(database))
        {
            
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

            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (@Guid, @Record, @FilePath, @MimeType)", record);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", record.Metadata.Values);
            });
        }

        public void Set(IEnumerable<IFileRecord> records)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (@Guid, @Record, @FilePath, @MimeType)", records);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", records.SelectMany(record => record.Metadata.Values));
            });
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

            this.backingDatabase.Execute(@"DELETE FROM files WHERE uuid = @guid; 
                                           DELETE FROM metadata WHERE record = @guid", new { guid });
        }

        public void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute(@"DELETE FROM files WHERE uuid IN @guids; 
                                           DELETE FROM metadata WHERE record IN @guids", new { guids });
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
            return this.GetMultipleByQuery(sql, new { guids }); //todo test no idea if this works
        }

        public IEnumerable<IFileRecord> GetAllRecords()
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
                @"SELECT * FROM files WHERE path = @filePath;
                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE path = @filePath)";
            return this.GetSingleByQuery(sql, new {filePath});
        }

        public IFileRecord Get(Guid recordGuid)
        {
            const string sql =
                            @"SELECT * FROM files WHERE uuid = @recordGuid;
                              SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid = @recordGuid)";

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
                            Path = file.path,
                            MimeType = file.mimetype
                        });
                        var metadatas = query.Read().Select(metadata => new
                        {
                            Guid = new Guid(metadata.uuid),
                            Record = new Guid(metadata.record),
                            Key = metadata.key,
                            Value = metadata.value
                        }).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value) as IRecordMetadata);
                        return (from f in files
                                let md = (from m in metadatas where m.Record == f.Guid select m)
                                         .ToDictionary(md => md.Key, md => md)
                                select new FileRecord(f.Guid, md, f.Path, f.MimeType)).ToList();
                    }
                    catch (DbException)
                    {
                        return new List<IFileRecord>();
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
                        var metadatas = query.Read().Select(metadata => new
                        {
                            Guid = new Guid(metadata.uuid),
                            Record = new Guid(metadata.record),
                            Key = metadata.key,
                            Value = metadata.value
                        }).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value) as IRecordMetadata)?
                        .ToDictionary(m => m.Key, m => m as IRecordMetadata); 
                        return new FileRecord(file.Game, metadatas, file.Path, file.MimeType);
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
