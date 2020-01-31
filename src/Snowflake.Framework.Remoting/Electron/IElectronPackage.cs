using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Electron
{
    /// <summary>
    /// Represents a loadable Electron package, that usually implements a user interface.
    /// </summary>
    public interface IElectronPackage
    {
        /// <summary>
        /// The author of the package.
        /// </summary>
        string Author { get; }
        /// <summary>
        /// The path of the package on disk.
        /// </summary>
        string PackagePath { get; }
        /// <summary>
        /// The "homepage", or entry file to use when loading this theme in Electron.
        /// </summary>
        string Entry { get; }
        /// <summary>
        /// The theme icon.
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// A description of this theme.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The name of this theme.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The version of this theme.
        /// </summary>
        string Version { get; }
    }
}
