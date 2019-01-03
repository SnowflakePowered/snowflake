using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Electron
{
    public interface IElectronPackage
    {
        string Author { get; }
        string PackagePath { get; }
        string Entry { get; }
        string Icon { get; }
        string Description { get; }
        string Name { get; }
        string Version { get; }
    }
}
