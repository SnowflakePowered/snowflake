using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    /// <summary>
    /// An option descriptor for options with type <see cref="ConfigurationOptionType.Selection"/>
    /// Describes the available options by introspecting the backing <see cref="EnumType"/>.
    /// </summary>
    public interface ISelectionOptionDescriptor
    {
        /// <summary>
        /// Gets the display name of this selection option.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the name of the enumeration value this selection option is backed by.
        /// </summary>
        string EnumName { get; }

        /// <summary>
        /// Gets the CLR enumeration type this selection value is of.
        /// </summary>
        Type EnumType { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this selection is to be shown to the user
        /// </summary>
        bool Private { get; }

        /// <summary>
        /// Gets the value this selection option appears in the serialized configuration.
        /// </summary>
        string SerializeAs { get; }

        /// <summary>
        /// Gets the numeric value that represents this selection in the context of the enumeration type.
        /// </summary>
        int NumericValue { get; }
    }
}
