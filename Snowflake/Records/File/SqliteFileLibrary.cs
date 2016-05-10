using System;
using System.Collections.Generic;
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
            SqlMapper.AddTypeHandler<Guid>(new GuidTypeHandler());
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
            var sql = @"
            SELECT * FROM metadata where record = @guid
            SELECT * FROM files where uuid = @guid
            ";
            return this.backingDatabase.Query<IFileRecord>(dbConnection =>
            {
                using (var query = dbConnection.QueryMultiple(sql, new { guid }))
                {
                    IDictionary<string, IRecordMetadata> metadata = query.Read<RecordMetadata>()
                         .ToDictionary(m => m.Key, m => m as IRecordMetadata);
                    dynamic file = query.Read().FirstOrDefault();
                    return file == null ? null : 
                    new FileRecord(file.uuid, file.game,                     metadata, file.path, file.mimetype);
                }
            });
        }

        public void Remove(Guid guid)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @guid", new { guid });
        }

        public IEnumerable<IFileRecord> SearchByMetadata(string key, string likeValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileRecord> GetByMetadata(string key, string exactValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileRecord> GetFilesForGame(IGameRecord game)
        {
           return this.GetFilesForGame(game.Guid);
        }

        public IEnumerable<IFileRecord> GetFilesForGame(Guid game)
        {
            var files = this.backingDatabase.Query(dbConnection => 
            (from file in dbConnection.Query("SELECT * FROM files where game = @game", new {game})
                select new
                {
                    Guid = new Guid(file.uuid),
                    Game = new Guid(file.game),
                    Path = file.path,
                    MimeType = file.mimetype
                })).ToList();
            var metadata = this.backingDatabase.Query<RecordMetadata>("SELECT * FROM metadata where record in @Ids",
                new {Ids = from f in files select f.Guid});
            return from f in files
                   let md = (from m in metadata where m.Record == f.Guid select m)
                            .ToDictionary(md => md.Key, md => md as IRecordMetadata)
                   select new FileRecord(f.Guid, f.Game, md, f.Path, f.MimeType);
        }

        public IFileRecord GetFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public IFileRecord GetFile(Guid recordGuid)
        {
            throw new NotImplementedException();
        }
    }
}
