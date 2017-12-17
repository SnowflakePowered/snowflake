using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Platform;

namespace Snowflake.Emulator.SystemFIles
{
    /// <summary>
    /// Provides System and BIOS files for emulator adapters.
    /// </summary>
    public interface ISystemFileProvider
    {
        /// <summary>
        /// Gets the specified BIOS file if available.
        /// </summary>
        /// <throws>FileNotFoundException if the BIOS file is not available.</throws>
        /// <param name="biosFile">The BIOS file specification.</param>
        /// <returns>The file as a stream.</returns>
        Stream GetSystemFile(IBiosFile biosFile);

        /// <summary>
        /// Gets the path of the specified BIOS file if available.
        /// </summary>
        /// <throws>FileNotFoundException if the BIOS file is not available.</throws>
        /// <param name="biosFile">The BIOS file specification.</param>
        /// <returns>The path to the file.</returns>
        DirectoryInfo GetSystemFilePath(IBiosFile biosFile);

        /// <summary>
        /// Adds the specified BIOS file to the registry.
        /// </summary>
        /// <exception cref="FileNotFoundException">If the system file is not found.</exception>
        /// <param name="biosFile">The BIOS file specification.</param>
        /// <param name="systemFilePath">The path to the file.</param>
        void AddSystemFile(IBiosFile biosFile, FileInfo systemFilePath);

        /// <summary>
        /// Adds the specified BIOS file to the registry.
        /// </summary>
        /// <exception cref="FileNotFoundException">If the system file is not found.</exception>
        /// <param name="biosFile">The BIOS file specification.</param>
        /// <param name="systemFileStream">The file as a stream.</param>
        void AddSystemFile(IBiosFile biosFile, Stream systemFileStream);
    }
}
