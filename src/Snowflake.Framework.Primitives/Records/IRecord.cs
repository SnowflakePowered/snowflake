﻿using System;
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
        /// Gets the metadata related to this metadata
        /// </summary>
        IMetadataCollection Metadata { get; }

        /// <summary>
        /// Gets a metadata assignable must have a guid.
        /// </summary>
        Guid Guid { get; }
    }
}
