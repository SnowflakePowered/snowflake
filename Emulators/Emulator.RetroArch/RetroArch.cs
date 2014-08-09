using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition;
using Snowflake.Emulator;

namespace Emulator.RetroArch
{
    public sealed class RetroArch : ExecutableEmulator
    {
        
        public RetroArch() : base(Assembly.GetExecutingAssembly())
        {
            this.InitConfiguration();
        }
  
        protected override ProcessStartInfo GetProcessStartInfo(string platformId, string fileName)
        {
            return this.GetProcessStartInfo(platformId, fileName,
                @"-L cores\" + this.PluginConfiguration.Configuration["cores"][platformId] + " " + fileName);
        }
        

    }
}
