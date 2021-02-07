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
        /// If the directory already exists, then it is considered a directory.
        /// Otherwise, this acts as a file.
        /// </summary>
        Either,
        /// <summary>
        /// This path is not virtualized and points to a path on the underlying filesystem.
        /// No attempt will be made to resolve the path.
        /// 
        /// This is mostly useful for specifying paths to BIOS files or some other non-user
        /// configurable path.
        /// </summary>
        Raw,
    }
}
