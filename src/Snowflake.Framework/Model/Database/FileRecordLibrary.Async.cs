using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Filesystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using System.Threading.Tasks;

namespace Snowflake.Model.Database
{
    internal partial class FileRecordLibrary : RecordLibrary<IFileRecord>
    {
        public async Task RegisterFileAsync(IFile file, string mimetype)
        {
            using var context = new DatabaseContext(this.Options.Options);
            var record = await context.FileRecords.FindAsync(file.FileGuid);
            if (record != null)
            {
                record.MimeType = mimetype;
                context.Entry(record).State = EntityState.Modified;
            }
            else
            {
                await context.FileRecords.AddAsync((file, mimetype).AsModel());
            }

            await context.SaveChangesAsync();
        }

        public async Task<IFileRecord?> GetRecordAsync(IFile file)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var record = await context.FileRecords.Include(f => f.Metadata)
                .SingleOrDefaultAsync(f => f.RecordID == file.FileGuid);
            if (record != null)
            {
                return new FileRecord(file, record.MimeType,
                    record.Metadata.AsMetadataCollection(file.FileGuid));
            }

            return null;
        }

        public async IAsyncEnumerable<IFileRecord> GetFileRecordsAsync(IIndelibleDirectory directoryRoot)
        {
            await using var context = new DatabaseContext(this.Options.Options);
            var files = directoryRoot.EnumerateFilesRecursive().ToList();
            var records = context.FileRecords
                .Include(r => r.Metadata)
                .Where(r => files.Select(f => f.FileGuid).Contains(r.RecordID));
            foreach (var r in records)
            {
                yield return new FileRecord(files.First(f => f.FileGuid == r.RecordID),
                    r.MimeType, r.Metadata.AsMetadataCollection(r.RecordID));
            }
        }
    }
}
