using System;
using Snowflake.Utility;

namespace Snowflake.Records.Metadata
{
    public class RecordMetadata : IRecordMetadata
    {
        public string Key { get; }
        public string Value { get; }
        public Guid MetadataGuid { get; }
        public Guid Record { get; }

        public RecordMetadata(string key, string value, Guid record)
        {
            this.Key = key;
            this.Value = value;
            this.Record = record;
            this.MetadataGuid = GuidUtility.Create(this.Record, this.Key);
        }

        public RecordMetadata(string key, string value, IRecord record) : this(key, value, record.Guid)
        {
        }

        public bool Equals(IRecordMetadata metadata) => this.MetadataGuid == metadata.MetadataGuid;
        public override int GetHashCode() => this.MetadataGuid.GetHashCode();

        public override bool Equals(object metadata)
        {
            IRecordMetadata m = metadata as IRecordMetadata;

            if (m == null)
            {
                return false;
            }

            // Return true if the fields match:
            return m.MetadataGuid == this.MetadataGuid;
        }

        public static bool operator ==(RecordMetadata metadataX, IRecordMetadata metadataY) => metadataX.Equals(metadataY);
        public static bool operator !=(RecordMetadata metadataX, IRecordMetadata metadataY) => !(metadataX == metadataY);
    }
}
