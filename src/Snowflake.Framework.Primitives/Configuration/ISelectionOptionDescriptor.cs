using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    public interface ISelectionOptionDescriptor
    {
        /// <summary>
        /// The display name of this selection option.
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// The name of the enumeration value this selection option is backed by.
        /// </summary>
        string EnumName { get; }
        /// <summary>
        /// The CLR enumeration type this selection value is of.
        /// </summary>
        Type EnumType { get; }
        /// <summary>
        /// Whether or not this selection is to be shown to the user
        /// </summary>
        bool Private { get; }
        /// <summary>
        /// The value this selection option appears in the serialized configuration.
        /// </summary>
        string SerializeAs { get; }
        /// <summary>
        /// The numeric value that represents this selection in the context of the enumeration type.
        /// </summary>
        int NumericValue { get; }
    }
}
