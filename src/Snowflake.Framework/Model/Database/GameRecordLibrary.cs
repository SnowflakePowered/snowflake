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
using Snowflake.Records.Metadata;

namespace Snowflake.Model.Database
{
    internal class GameRecordLibrary : RecordLibrary<IGameRecord>
    {
        public GameRecordLibrary(DbContextOptionsBuilder<DatabaseContext> options)
            : base(options)
        {
        }

        public IEnumerable<IGameRecord> GetAllRecords()
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var records = context.GameRecords.Include(r => r.Metadata)
                    .Select(record => new GameRecord(record.Platform, record.RecordID,
                            record.Metadata.AsMetadataCollection(record.RecordID)))

                        .ToList();
                return records;
            }
        }

        public IEnumerable<IGameRecord> GetRecords(Expression<Func<IGameRecord, bool>> predicate)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var records = context.GameRecords.Include(r => r.Metadata)
                 .Select(record => new GameRecord(record.Platform, record.RecordID,
                         record.Metadata.AsMetadataCollection(record.RecordID)))
                 .Where(predicate).ToList();
                return records;
            }
        }

        public async Task<IEnumerable<IGameRecord>> GetRecordsAsync(Expression<Func<IGameRecord, bool>> predicate)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var records = await context.GameRecords.Include(r => r.Metadata)
                 .Select(record => new GameRecord(record.Platform, record.RecordID,
                         record.Metadata.AsMetadataCollection(record.RecordID)))
                 .Where(predicate).ToListAsync();
                return records;
            }
        }

        public IGameRecord? GetRecord(Guid guid)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var record = context.GameRecords.Include(r => r.Metadata)
                    .SingleOrDefault(g => g.RecordID == guid);
                if (record != null)
                {
                    return new GameRecord(record.Platform, record.RecordID,
                            record.Metadata.AsMetadataCollection(record.RecordID));
                }
                return null;

            }
        }

        public IGameRecord CreateRecord(PlatformId platformId)
        {
            var recordGuid = Guid.NewGuid();
            var record = new GameRecord(platformId, recordGuid, new MetadataCollection(recordGuid));
            using (var context = new DatabaseContext(this.Options.Options))
            {
                context.GameRecords.Add(record.AsModel());
                context.SaveChanges();
            }
            return record;
        }
    }
}
