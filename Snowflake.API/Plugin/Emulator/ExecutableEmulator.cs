using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface.Plugin;
using Snowflake.API.Plugin;
using System.Reflection;

namespace Snowflake.API.Plugin.Emulator
{
    public abstract class ExecutableEmulator : BasePlugin, IEmulator
    {
        public ExecutableEmulator(Assembly pluginAssembly):base(pluginAssembly)
        {
        }
        public abstract void Run(string uuid);


   }
}


