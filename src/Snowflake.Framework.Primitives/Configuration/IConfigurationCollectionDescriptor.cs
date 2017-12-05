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
        /// Gets the file outputs keyed on the file reference key to the file name
        /// The implementation should ensure this is immutable.
        /// <seealso cref="ConfigurationFileAttribute"/>
        /// <seealso cref="IConfigurationFile"/>
        /// </summary>
        IDictionary<string, IConfigurationFile> Outputs { get; }

        /// <summary>
        /// Gets the list of names of property section names.
        /// The implementation should ensure this is immutable and enumerate in the same order
        /// as the properties were described in the collection type.
        /// </summary>
        IEnumerable<string> SectionKeys { get; }

        /// <summary>
        /// Gets the destination file reference key for a section
        /// </summary>
        /// <param name="sectionKey">The property name of the section</param>
        /// <returns>The file reference key</returns>
        /// <seealso cref="Outputs"/>
        string GetDestination(string sectionKey);
    }
}