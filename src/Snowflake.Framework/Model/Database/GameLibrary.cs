using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Contexts;
using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal class GameLibrary
    {
        private DbContextOptions<GameRecordContext> Options { get; }
        public GameLibrary(DbContextOptions<GameRecordContext> options)
        {
            this.Options = options;
            using (var context = new GameRecordContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        public IEnumerable<IGameRecord> GetAllRecords()
        {
            using (var context = new GameRecordContext(this.Options))
            {
                return (from record in context.GameRecords
                        where record.RecordType == "game"
                        let metadata = context.Metadata.Where(m => m.RecordID == record.RecordID)
                        select new GameRecord(record.Platform, record.RecordID, 
                            metadata.AsMetadataCollection(record.RecordID)))
                        .ToList();
            }
        }

        public void AddRecord(IGameRecord record)
        {
            using (var context = new GameRecordContext(this.Options))
            {
                context.GameRecords.Add(record.AsModel());
                context.SaveChanges();
            }
        }

        public void UpdateRecord(IGameRecord record)
        {
            using (var context = new GameRecordContext(this.Options))
            {
                foreach 
                    (var metadata in
                    context.Metadata.Where(m => m.RecordID == record.RecordId))
                {
                    if (!record.Metadata.ContainsKey(metadata.MetadataKey))
                    {
                        context.Entry(metadata).State = EntityState.Deleted;
                    } else if (record.Metadata[metadata.MetadataKey] != metadata.MetadataValue)
                    {
                        metadata.MetadataValue = record.Metadata[metadata.MetadataKey];
                        context.Entry(metadata).State = EntityState.Modified;

                    }
                }

                foreach (var metadata in record.Metadata.Values)
                {
                    var model = context.Metadata?.Find(metadata.Guid);
                    if (model != null) continue;
                    context.Metadata!.Add(new RecordMetadataModel()
                    {
                        RecordID = metadata.Record,
                        RecordMetadataID = metadata.Guid,
                        MetadataValue = metadata.Value,
                        MetadataKey = metadata.Key
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
