using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Utility;

namespace Snowflake.Records.Metadata
{
    internal class SqliteMetadataLibrary : IMetadataLibrary
    {
        private readonly SqliteDatabase backingDatabase;
        public SqliteMetadataLibrary(SqliteDatabase database)
        {
            this.backingDatabase = database;
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("metadata",
                "uuid UUID PRIMARY KEY",
                "record UUID",
                "key TEXT",
                "value TEXT");
        }

        public void Set(IRecordMetadata metadata)
        {

            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", metadata);

        }

        public void Set(IEnumerable<IRecordMetadata> metadata)
        {
            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (@Guid, @Record, @Key, @Value)", metadata);
        }

        public void Remove(IRecordMetadata metadata)
        {
            this.Remove(metadata.Guid);
        }

        public void Remove(IEnumerable<IRecordMetadata> metadata)
        {
            this.Remove(metadata.Select(m => m.Guid));
        }

        public void Remove(Guid metadataId)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @metadataId", new { metadataId });
        }

        //todo test
        public void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @metadataId",
                guids.Select(metadataId => new {metadataId}));
        }

        public IDictionary<string, IRecordMetadata> GetAllForElement(Guid target)
        {
            return this.QueryMetadata(@"SELECT * FROM metadata WHERE record = @target", new { target }).ToDictionary(m => m.Key, m => m);
              
        }


        public IEnumerable<IRecordMetadata> Search(string key, string likeValue)
        {

            return this.QueryMetadata(@"SELECT * FROM metadata WHERE key = @key AND value LIKE @likeValue",
                    new { key, likeValue = $"%{likeValue}%" });
        }

        public IEnumerable<IRecordMetadata> Get(string key, string exactValue)
        {
            return this.QueryMetadata(@"SELECT * FROM metadata WHERE key = @key AND value = @exactValue",
                    new { key, exactValue });
        }


        public IEnumerable<IRecordMetadata> GetAllRecords()
        {
            return this.QueryMetadata(@"SELECT * FROM metadata", null);
        }

        public IRecordMetadata Get(Guid metadataId)
        {
            return this.QueryMetadata(@"SELECT * FROM metadata WHERE uuid = @metadataId LIMIT 1", new { metadataId }).FirstOrDefault();
        }

        public IEnumerable<IRecordMetadata> Get(IEnumerable<Guid> guids)
        {
            return this.QueryMetadata(@"SELECT * FROM metadata WHERE uuid in @guids", new { guids });
        }

        public IRecordMetadata Get(string key, Guid recordId)
        {
            return this.QueryMetadata($@"SELECT * FROM metadata WHERE record = @recordId AND key = @key LIMIT 1",
                    new { key, recordId }).FirstOrDefault();
        }


        private IEnumerable<IRecordMetadata> QueryMetadata(string queryString, object param)
        {
            return this.backingDatabase.Query(conn =>
            {
                var query = conn.Query<MetadataRecord>(queryString, param);
                return query.Select(m => new RecordMetadata(new Guid(m.uuid), new Guid(m.record), m.key, m.value) as IRecordMetadata);
            });
        }
    }

    class MetadataRecord
    {
        public byte[] uuid;
        public byte[] record;
        public string value;
        public string key;
    }
}
