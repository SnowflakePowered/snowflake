using System;
using System.Collections.Generic;

namespace Snowflake.Framework.Remoting.Electron
{
    public interface IElectronPackageProvider
    {
        IEnumerable<IElectronPackage> Interfaces { get; }
    }
}
