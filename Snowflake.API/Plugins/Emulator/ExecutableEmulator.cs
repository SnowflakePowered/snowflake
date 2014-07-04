using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;
using Snowflake.API.Plugins;

namespace Snowflake.API.Plugins.Emulator
{
    public class ExecutableEmulator : Plugin, IEmulator
    {
        public ExecutableEmulator(string pluginName, string baseDirectory, string executableName):base(pluginName)
        {
        }
        public virtual void Run(string fileName, string gameConfiguration);
   }
}


