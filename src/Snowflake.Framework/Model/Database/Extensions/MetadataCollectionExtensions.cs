using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Records.Metadata;
using System.Linq;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database.Extensions
{
    internal static class MetadataCollectionExtensions
    {
        public static List<RecordMetadataModel> AsModel(this IMetadataCollection @this)
        {
            return (from metadata in @this.Values
                    select new RecordMetadataModel()
                    {
                        RecordID = metadata.Record,
                        RecordMetadataID = metadata.Guid,
                        MetadataValue = metadata.Value,
                        MetadataKey = metadata.Key
                    }).ToList();
        }

        public static IMetadataCollection AsMetadataCollection(this IEnumerable<RecordMetadataModel> @this,
            Guid recordGuid)
        {
            var collection = new MetadataCollection(recordGuid);
            
            foreach (var metadata in @this)
            {
                collection.Add(new RecordMetadata(metadata.MetadataKey, 
                    metadata.MetadataValue, recordGuid));
            }

            return collection;
        }
    }
}
