using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
	/// <summary>
    /// Internal constructors for Dapper materialization
    /// </summary>
    public partial class RecordMetadata
    {
        internal RecordMetadata(byte[] uuid, byte[] record, string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.Record = new Guid(record);
            this.Guid = new Guid(uuid);
        }

        internal RecordMetadata(object uuid, object record, string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.Record = new Guid((byte[])record);
            this.Guid = new Guid((byte[])uuid);
        }
    }
}
