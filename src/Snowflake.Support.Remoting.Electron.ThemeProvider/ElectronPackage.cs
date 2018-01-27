using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Framework.Remoting.Electron;

namespace Snowflake.Support.Remoting.Electron
{
    public class ElectronPackage : IElectronPackage
    {
        public string Author { get; set; }
        public string PackagePath { get; set; }
        public string Entry { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
    }
}
