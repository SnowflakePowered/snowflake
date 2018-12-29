using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    public class MetadataCollection : Dictionary<string, IRecordMetadata>, IMetadataCollection
    {
        /// <inheritdoc/>
        public IRecordMetadata this[Guid guid] => this.First(metadata => metadata.Value.Guid == guid).Value;

        /// <inheritdoc/>
        public void Add(IDictionary<string, IRecordMetadata> existingMetadata)
        {
            foreach (var metadata in existingMetadata)
            {
                this.Add(metadata.Key, metadata.Value.Value);
            }
        }

        /// <inheritdoc/>
        string? IMetadataCollection.this[string key]
        {
            get { return this.ContainsKey(key) ? this[key].Value : null; }
            set
            {
                if (value != null)
                {
                    this[key] = new RecordMetadata(key, value, this.Record);
                }
                else
                {
                    this.Remove(key);
                }
            }
        }

        /// <inheritdoc/>
        public Guid Record { get; }

        internal MetadataCollection(Guid record)
        {
            this.Record = record;
        }

        internal MetadataCollection(Guid record, IDictionary<string, IRecordMetadata> recordMetadata)
            : base(recordMetadata)
        {
            this.Record = record;
        }

        /// <inheritdoc/>
        public void Add(IRecordMetadata recordMetadata)
        {
            this.Add(recordMetadata.Key, recordMetadata);
        }

        /// <inheritdoc/>
        public void Add(string key, string value)
        {
            this.Add(new RecordMetadata(key, value, this.Record));
        }
    }
}
