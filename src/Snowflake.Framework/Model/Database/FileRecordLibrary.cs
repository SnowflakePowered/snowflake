using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database.Extensions;
using Snowflake.Model.Database.Models;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;

namespace Snowflake.Model.Database
{
    internal class FileRecordLibrary : RecordLibrary<IFileRecord>
    {
        public FileRecordLibrary(DbContextOptionsBuilder<DatabaseContext> options)
            : base(options)
        {
        }

        public void RegisterFile(IFile file, string mimetype)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var record = context.FileRecords.Find(file.FileGuid);
                if (record != null)
                {
                    record.MimeType = mimetype;
                    context.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    context.FileRecords.Add((file, mimetype).AsModel());
                }
                context.SaveChanges();
            }
        }
        public IFileRecord? GetRecord(IFile file)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
                var record = context.FileRecords.Include(f => f.Metadata)
                    .SingleOrDefault(f => f.RecordID == file.FileGuid);
                if (record != null)
                {
                    return new FileRecord(file, record.MimeType,
                        record.Metadata.AsMetadataCollection(file.FileGuid));
                }
                return null;

            }
        }
        public IEnumerable<IFileRecord> GetFileRecords(IDirectory directoryRoot)
        {
            using (var context = new DatabaseContext(this.Options.Options))
            {
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
}
