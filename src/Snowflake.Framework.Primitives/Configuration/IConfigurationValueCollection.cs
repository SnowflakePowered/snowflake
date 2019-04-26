using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a enumerable collection of configuration values.
    /// </summary>
    public interface IConfigurationValueCollection
        : IEnumerable<(string section, string option, IConfigurationValue value)>
    {
        /// <summary>
        /// The unique GUID for this collection of values.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Retrieve the value in the collection for the given option within the given section.
        /// </summary>
        /// <param name="descriptor">The descriptor for the section to examine.</param>
        /// <param name="option">The option key within the section of the configuration.</param>
        /// <returns>The configuration value of the provided option.</returns>
        IConfigurationValue? this[IConfigurationSectionDescriptor descriptor, string option] { get; }

        /// <summary>
        /// Retrieve a read-only view of configuration values for a given configuration section.
        /// </summary>
        /// <param name="descriptor">The descriptor for the section to examine.</param>
        /// <returns>A read-only view of configuration values, keyed on option keys of the provided section.</returns>
        IReadOnlyDictionary<string, IConfigurationValue> this[IConfigurationSectionDescriptor descriptor] { get; }
    }
}
