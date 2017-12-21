using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Persistence;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;

namespace Snowflake.Records.File
{
    internal class SqliteFileLibrary : SqliteRecordLibrary<IFileRecord>, IFileLibrary
    {
        /// <inheritdoc/>
        public override IMetadataLibrary MetadataLibrary { get; }
        private static readonly string[] columns = new[]
        {
           "path TEXT",
           "mimetype TEXT",
        };

        private readonly ISqlDatabase backingDatabase;
        public SqliteFileLibrary(ISqlDatabase database, IMetadataLibrary metadataLibrary)
            : base(database, "files", SqliteFileLibrary.columns)
        {
            this.backingDatabase = database;
            this.MetadataLibrary = metadataLibrary;
        }

        public SqliteFileLibrary(ISqlDatabase database)
            : this(database, new SqliteMetadataLibrary(database))
        {
        }

        /// <inheritdoc/>
        public override void Set(IFileRecord record)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, path, mimetype) 
                                         VALUES (@Guid, @FilePath, @MimeType)", record);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", record.Metadata.Values);
            });
        }

        /// <inheritdoc/>
        public override void Set(IEnumerable<IFileRecord> records)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, path, mimetype) 
                                         VALUES (@Guid, @FilePath, @MimeType)", records);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", records.SelectMany(record => record.Metadata.Values));
            });

            // since metadata are unique across records, we can do this.
        }

        /// <inheritdoc/>
        public override void Remove(IFileRecord record)
        {
            this.Remove(record.Guid);
        }

        /// <inheritdoc/>
        public override void Remove(IEnumerable<IFileRecord> records)
        {
            this.Remove(records.Select(record => record.Guid));
        }

        /// <inheritdoc/>
        public override void Remove(Guid guid)
        {
            this.backingDatabase.Execute(@"DELETE FROM files WHERE uuid = @guid", new { guid });
        }

        /// <inheritdoc/>
        public override void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute(@"DELETE FROM files WHERE uuid IN @guids", new { guids });
        }

        /// <inheritdoc/>
        public override IEnumerable<IFileRecord> SearchByMetadata(string key, string likeValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value LIKE @likeValue);
                                 SELECT * FROM metadata WHERE key = @key AND value LIKE @likeValue";
            return this.GetMultipleByQuery(sql, new { key, likeValue = $"%{likeValue}%" });
        }

        /// <inheritdoc/>
        public override IEnumerable<IFileRecord> GetByMetadata(string key, string exactValue)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN (SELECT record FROM metadata WHERE key = @key AND value = @exactValue);
                                 SELECT * FROM metadata WHERE key = @key AND value = @exactValue";
            return this.GetMultipleByQuery(sql, new { key, exactValue });
        }

        /// <inheritdoc/>
        public override IEnumerable<IFileRecord> Get(IEnumerable<Guid> guids)
        {
            const string sql = @"SELECT * FROM files WHERE uuid IN @guids;
                                 SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid IN @guids)";
            return this.GetMultipleByQuery(sql, new { guids });
        }

        /// <inheritdoc/>
        public override IEnumerable<IFileRecord> GetAllRecords()
        {
            const string sql = @"SELECT * FROM files;
                                 SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files)";
            return this.GetMultipleByQuery(sql, null);
        }

        /// <inheritdoc/>
        public IFileRecord Get(string filePath)
        {
            const string sql =
                @"SELECT * FROM files WHERE path = @filePath;
                SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE path = @filePath)";
            return this.GetSingleByQuery(sql, new { filePath });
        }

        /// <inheritdoc/>
        public override IFileRecord Get(Guid recordGuid)
        {
            const string sql =
                            @"SELECT * FROM files WHERE uuid = @recordGuid;
                              SELECT * FROM metadata WHERE record IN (SELECT uuid FROM files WHERE uuid = @recordGuid)";

            return this.GetSingleByQuery(sql, new { recordGuid });
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
                        var files = query.Read().Select(file => (
                            Guid: new Guid(file.uuid),
                            Path: file.path,
                            MimeType: file.mimetype));
                        var metadatas = query.Read().Select(metadata => (
                            Guid: new Guid(metadata.uuid),
                            Record: new Guid(metadata.record),
                            Key: (string)metadata.key,
                            Value: (string)metadata.value)).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value))?
                        .Cast<IRecordMetadata>();
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
                        if (_file == null)
                        {
                            return null;
                        }

                        var file = (
                            Guid: new Guid(_file.uuid),
                            Path: (string)_file.path,
                            MimeType: (string)_file.mimetype);
                        var metadatas = query.Read().Select(metadata => (
                            Guid: new Guid(metadata.uuid),
                            Record: new Guid(metadata.record),
                            Key: (string)metadata.key,
                            Value: (string)metadata.value)).Select(m => new RecordMetadata(m.Guid, m.Record, m.Key, m.Value))?
                        .Cast<IRecordMetadata>()?
                        .ToDictionary(m => m.Key, m => m);
                        return new FileRecord(file.Guid, metadatas, file.Path, file.MimeType);
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
