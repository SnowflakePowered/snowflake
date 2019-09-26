using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records;

namespace Snowflake.Model.Database
{
    internal abstract class RecordLibrary<TRecord>
        where TRecord : class, IRecord
    {
        protected DbContextOptionsBuilder<DatabaseContext> Options { get; private set; }

        protected RecordLibrary(DbContextOptionsBuilder<DatabaseContext> options)
        {
            this.Options = options;
            using var context = new DatabaseContext(options.Options);
            context.Database.Migrate();
        }

        public void DeleteRecord(TRecord record)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var recordToDelete = context.Records.SingleOrDefault(r => r.RecordID == record.RecordID);
            if (record == null) return;
            context.Entry(recordToDelete).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void UpdateRecord(TRecord record)
        {
            using var context = new DatabaseContext(this.Options.Options);
            foreach (var metadata in context.Metadata.Where(m => m.RecordID == record.RecordID))
            {
                if (!record.Metadata.ContainsKey(metadata.MetadataKey))
                {
                    context.Entry(metadata).State = EntityState.Deleted;
                }
                else if (record.Metadata[metadata.MetadataKey] != metadata.MetadataValue)
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
