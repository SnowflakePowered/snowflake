using System;
using System.Collections.Generic;
using System.Linq;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Scraping.RecordTraversers.Extensions;

namespace Snowflake.Support.Scraping.RecordTraversers
{
    public class FileRecordTraverser : ITraverser<IFileRecord>
    {
        public IEnumerable<IFileRecord> Traverse(ISeed relativeRoot, ISeedRootContext context)
        {
            foreach (var fileSeed in context.GetChildren(relativeRoot).Where(s => s.Content.Type == "file"))
            {
                var children = context.GetChildren(fileSeed);
                var mimetypeSeed = children.FirstOrDefault(s => s.Content.Type == "mimetype");
                if (mimetypeSeed == null)
                {
                    continue;
                }

                var metadataSeeds = context.GetDescendants(fileSeed)
                    .DistinctBy(p => p.Content.Type).Select(p => p.Content);
           //     var fileRecord = new FileRecord(fileSeed.Content.Value, mimetypeSeed.Content.Value);
                foreach (var content in metadataSeeds)
                {
                   // fileRecord.Metadata[$"file_{content.Type}"] = content.Value;
                }

                yield return null;
            }
        }
    }
}
