using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Utility;

namespace Snowflake.Records.Game
{
    public class SqliteGameLibrary : IGameLibrary
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
                "uuid TEXT PRIMARY KEY");
        }
        public void Set(IGameRecord record)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES @Guid", record);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                         VALUES (@Guid, @GameRecord, @FilePath, @MimeType)", record.Files);
                dbConnection.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", 
                                        record.Metadata.Values.Concat(record.Files.SelectMany(m => m.Metadata.Values)));
            });
        }

        public void Set(IEnumerable<IGameRecord> records)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                var gameRecords = records as IList<IGameRecord> ?? records.ToList();
                dbConnection.Execute(@"INSERT OR REPLACE INTO games(uuid) VALUES @Guid", gameRecords);
                dbConnection.Execute(@"INSERT OR REPLACE INTO files(uuid, game, path, mimetype) 
                                       VALUES (@Guid, @GameRecord, @FilePath, @MimeType)", 
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

        public void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute(@"DELETE FROM games WHERE uuid IN @guids;
                                           DELETE FROM metadata WHERE record IN @guids", new { guids });
        }

        public IGameRecord Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> Get(IEnumerable<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid guid)
        {
            this.backingDatabase.Execute(@"DELETE FROM games WHERE uuid = @guid;
                                           DELETE FROM metadata WHERE record = @guid", new { guid });
            //because file record guids are derived from game library, they can be safely left alone
        }

        public IEnumerable<IGameRecord> SearchByMetadata(string key, string likeValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetByMetadata(string key, string exactValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

       
        public IEnumerable<IGameRecord> GetGameRecords()
        {
            throw new NotImplementedException();
        }

        public IGameRecord GetGameByUuid(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetGamesByTitle(string nameSearch)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        {
            throw new NotImplementedException();
        }
    }
}
