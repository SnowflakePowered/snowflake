using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records;

namespace Snowflake.Model.Database
{
    internal abstract partial class RecordLibrary<TRecord>
        where TRecord : class, IRecord
    {
        public async Task DeleteRecordAsync(TRecord record)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var recordToDelete = await context.Records.SingleOrDefaultAsync(r => r.RecordID == record.RecordID);
            if (record == null) return;
            context.Entry(recordToDelete).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task UpdateRecordAsync(TRecord record)
        {
            await using var context = new DatabaseContext(this.Options.Options);
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
                var model = await context.Metadata.FindAsync(metadata.Guid);
                if (model != null) continue;
                await context.Metadata.AddAsync(new RecordMetadataModel()
                {
                    RecordID = metadata.Record,
                    RecordMetadataID = metadata.Guid,
                    MetadataValue = metadata.Value,
                    MetadataKey = metadata.Key
                });
            }

           await context.SaveChangesAsync();
        }
    }
}
