using System;
using System.Collections.Generic;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Describes a configuration collection.
    /// </summary>
    public interface IConfigurationCollectionDescriptor
    {
        /// <summary>
        /// Gets the list of names of property section names.
        /// The implementation should ensure this is immutable and enumerate in the same order
        /// as the properties were described in the collection type.
        /// </summary>
        IEnumerable<string> SectionKeys { get; }
    }
}
