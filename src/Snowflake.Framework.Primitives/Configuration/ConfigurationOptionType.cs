using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Game;

namespace Snowflake.Configuration
{
    /// <summary>
    /// The types of configuration options.
    /// </summary>
    public enum ConfigurationOptionType
    {
        /// <summary>
        /// This option will be treated as a UTF-8 string.
        /// </summary>
        String,
        /// <summary>
        /// This option will be treated as a file system path.
        ///
        /// If this is a game configuration, this will usually be relative
        /// to the game file system root.
        /// </summary>
        Path,
        /// <summary>
        /// This option is a true or false value.
        /// </summary>
        Boolean,
        /// <summary>
        /// This option is an integer value.
        /// </summary>
        Integer,
        /// <summary>
        /// This option is a rational value with unspecified precision.
        /// </summary>
        Decimal,
        /// <summary>
        /// This value is a choice out of a limited set of choices.
        /// </summary>
        Selection,
        /// <summary>
        /// This value is a GUID representing a file record
        /// </summary>
        Resource,
    }
}
