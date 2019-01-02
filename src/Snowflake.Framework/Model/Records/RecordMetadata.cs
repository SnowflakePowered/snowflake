using System;
using Snowflake.Model.Records.Utility;

namespace Snowflake.Model.Records
{
    /// <inheritdoc/>
    public sealed partial class RecordMetadata : IRecordMetadata
    {
        /// <inheritdoc/>
        public string Key { get; }

        /// <inheritdoc/>
        public string Value { get; }

        /// <inheritdoc/>
        public Guid Guid { get; }

        /// <inheritdoc/>
        public Guid Record { get; }

        public RecordMetadata(string key, string value, Guid record)
        {
            this.Key = key;
            this.Value = value;
            this.Record = record;
            this.Guid = GuidCreator.Create(this.Record, this.Key);
        }

        public RecordMetadata(string key, string value, IRecord record)
            : this(key, value, record.RecordId)
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

        /// <inheritdoc/>
        public override int GetHashCode() => this.Guid.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object metadata)
        {
            if (!(metadata is IRecordMetadata m))
            {
                return false;
            }

            // Return true if the fields match:
            return m.Guid == this.Guid;
        }

        public static bool operator ==(RecordMetadata metadataX, IRecordMetadata metadataY) =>
            metadataX.Equals(metadataY);

        public static bool operator !=(RecordMetadata metadataX, IRecordMetadata metadataY) =>
            !(metadataX == metadataY);
    }
}
