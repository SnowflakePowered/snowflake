using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using System.IO;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides the application content directory.
    /// <para>
    /// Often this is unneeded. Prefer the Snowflake <see cref="Snowflake.Filesystem"/> API if possible.
    /// If you are writing a provisioned <see cref="IPlugin"/>, a working directory has already been provided to you through the
    /// Filesystem API through <see cref="IPluginProvision"/>.
    /// </para>
    /// <para>
    /// If you are writing a service, you may also be interested in the Zio.IFileSystem service for safer filesystem access.
    /// </para>
    /// </summary>
    public interface IContentDirectoryProvider
    {
        /// <summary>
        /// Gets the application content directory.
        /// </summary>
        DirectoryInfo ApplicationData { get; }
    }
}
