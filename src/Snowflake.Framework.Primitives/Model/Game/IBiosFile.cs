using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Game
{
    /// <summary>
    /// Represents a BIOS file of a Stone <see cref="IPlatformInfo"/> definition.
    /// </summary>
    public interface IBiosFile
    {
        /// <summary>
        /// Gets the canonical file name of the BIOS file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the unique MD5 hash of the BIOS file.
        /// </summary>
        string Md5Hash { get; }
    }
}
