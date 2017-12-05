using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides the application content directory.
    /// </summary>
    public interface IContentDirectoryProvider
    {
        /// <summary>
        /// Gets the application content directory.
        /// </summary>
        DirectoryInfo ApplicationData { get; }
    }
}
