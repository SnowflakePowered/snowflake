using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    public class MetadataCollection : Dictionary<string, IRecordMetadata>, IMetadataCollection
    {
        public IRecordMetadata this[Guid guid]
        {
            get
            {
                return this.First(metadata => metadata.Value.Guid == guid).Value;
            }
            set
            {
                this[this[guid].Key] = value;
            }
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
