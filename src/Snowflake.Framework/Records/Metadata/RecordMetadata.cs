using System;
using Snowflake.Utility;

namespace Snowflake.Records.Metadata
{
    public partial class RecordMetadata : IRecordMetadata
    {
        public string Key { get; }
        public string Value { get; }
        public Guid Guid { get; }
        public Guid Record { get; }

        public RecordMetadata(string key, string value, Guid record)
        {
            this.Key = key;
            this.Value = value;
            this.Record = record;
            this.Guid = GuidUtility.Create(this.Record, this.Key);
        }

        public RecordMetadata(string key, string value, IRecord record) : this(key, value, record.Guid)
        {
        }

        internal RecordMetadata(Guid uuid, Guid record, string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.Record = record;
            this.Guid = uuid;
        }

        public bool Equals(IRecordMetadata metadata) => this.Guid == metadata.Guid;
        public override int GetHashCode() => this.Guid.GetHashCode();

        public override bool Equals(object metadata)
        {
            IRecordMetadata m = metadata as IRecordMetadata;

            if (m == null)
            {
                return false;
            }

            // Return true if the fields match:
            return m.Guid == this.Guid;
        }

        public static bool operator ==(RecordMetadata metadataX, IRecordMetadata metadataY) => metadataX.Equals(metadataY);
        public static bool operator !=(RecordMetadata metadataX, IRecordMetadata metadataY) => !(metadataX == metadataY);
    }
}
