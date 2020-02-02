using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database
{
    internal partial class GameRecordLibrary : RecordLibrary<IGameRecord>
    {
        public IAsyncEnumerable<IGameRecord> GetAllRecordsAsync()
        {
            var context = new DatabaseContext(this.Options.Options);
            var records = context.GameRecords.Include(r => r.Metadata)
                .Select(record => new GameRecord(record.PlatformID, record.RecordID,
                record.Metadata.AsMetadataCollection(record.RecordID))).AsAsyncEnumerable();
            return records;
        }

        public IAsyncEnumerable<IGameRecord> QueryRecordsAsync(Expression<Func<IGameRecordQuery, bool>> predicate)
        {
            var context = new DatabaseContext(this.Options.Options);
            return context.GameRecords
                .Include(r => r.Metadata)
                .Where(predicate)
                .Select(record => new GameRecord(record.PlatformID, record.RecordID,
                record.Metadata.AsMetadataCollection(record.RecordID))).AsAsyncEnumerable();
        }

        public async Task<IGameRecord?> GetRecordAsync(Guid guid)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var record = await context.GameRecords.Include(r => r.Metadata).SingleOrDefaultAsync(g => g.RecordID == guid);
            if (record != null)
            {
                return new GameRecord(record.PlatformID, record.RecordID,
                    record.Metadata.AsMetadataCollection(record.RecordID));
            }

            return null;
        }

        public async Task<IGameRecord> CreateRecordAsync(PlatformId platformId)
        {
            var recordGuid = Guid.NewGuid();
            var record = new GameRecord(platformId, recordGuid, new MetadataCollection(recordGuid));
            await using (var context = new DatabaseContext(this.Options.Options))
            {
                await context.GameRecords.AddAsync(record.AsModel());
                await context.SaveChangesAsync();
            }

            return record;
        }
    }
}
