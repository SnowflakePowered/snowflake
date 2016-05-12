using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Records
{
    /// <summary>
    /// Represents a class that can have metadata assigned to it.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// The metadata related to this metadata
        /// </summary>
        IDictionary<string, IRecordMetadata> Metadata { get; }
        /// <summary>
        /// A metadata assignable must have a guid.
        /// </summary>
        Guid Guid { get; }

    }
}
