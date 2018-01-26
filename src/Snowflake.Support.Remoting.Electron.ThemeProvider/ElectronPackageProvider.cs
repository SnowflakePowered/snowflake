using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.Electron;
using Snowflake.Loader;

namespace Snowflake.Support.Remoting.Electron
{
    public class ElectronPackageProvider : IElectronPackageProvider
    {
        public IEnumerable<IElectronPackage> Interfaces { get; private set; }

        public ElectronPackageProvider(ILogger logger, IModuleEnumerator enumerator)
        {
            var modules = enumerator.Modules.Where(m => m.Loader == "electron");
            var loader = new ElectronAsarLoader(logger);

            this.Interfaces = modules.SelectMany(m => loader.LoadModule(m)).ToList().ToImmutableList();
        }
    }
}
