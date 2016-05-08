using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Records.Metadata
{
    /// <summary>
    /// Represents a class that can have metadata assigned to it.
    /// </summary>
    public interface IMetadataAssignable
    {
        /// <summary>
        /// The metadata related to this metadata
        /// </summary>
        IDictionary<string, IMetadata> Metadata { get; }
        /// <summary>
        /// A metadata assignable must have a guid.
        /// </summary>
        Guid Guid { get; }
    }
}
