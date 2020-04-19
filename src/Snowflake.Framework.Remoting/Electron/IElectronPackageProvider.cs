using System;
using System.Collections.Generic;

namespace Snowflake.Remoting.Electron
{
    /// <summary>
    /// A service that provides access to Electron package manifests.
    /// </summary>
    public interface IElectronPackageProvider
    {
        /// <summary>
        /// A list of loaded Electron packages that implement user interfaces.
        /// </summary>
        IEnumerable<IElectronPackage> Interfaces { get; }
    }
}
