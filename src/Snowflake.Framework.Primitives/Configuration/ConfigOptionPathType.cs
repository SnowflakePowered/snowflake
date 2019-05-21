using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Game;

namespace Snowflake.Configuration
{
    /// <summary>
    /// The type of the configuration path if isPath is true.
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// This is not a path.
        /// </summary>
        NotPath,
        /// <summary>
        /// This path points to a file
        /// </summary>
        File,
        /// <summary>
        /// This path points to a directory
        /// </summary>
        Directory,
        /// <summary>
        /// This path points either a directory or a file.
        /// </summary>
        Either,
    }
}
