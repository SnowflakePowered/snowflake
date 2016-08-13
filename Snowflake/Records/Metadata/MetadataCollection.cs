using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    [Serializable]
    public class MetadataCollection : Dictionary<string, IRecordMetadata>, IMetadataCollection
    {
        public IRecordMetadata this[Guid guid] => this.First(metadata => metadata.Value.Guid == guid).Value;

        public void Add(IDictionary<string, IRecordMetadata> existingMetadata)
        {
            foreach (var metadata in existingMetadata)
            {
                this.Add(metadata.Key, metadata.Value.Value);
            }
        }

        string IMetadataCollection.this[string key]
        {
            get { return this[key].Value; }
            set { this[key] = new RecordMetadata(key, value, this.Record); }
        }

        public Guid Record { get; }

        public MetadataCollection(Guid record)
        {
            this.Record = record;
        }

        public MetadataCollection(Guid record, IDictionary<string, IRecordMetadata> recordMetadata) : base(recordMetadata)
        {
            this.Record = record;
        }

        public void Add(IRecordMetadata recordMetadata)
        {
            this.Add(recordMetadata.Key, recordMetadata);
        }

        public void Add(string key, string value)
        {
            this.Add(new RecordMetadata(key, value, this.Record));
        }
        
    }
}
