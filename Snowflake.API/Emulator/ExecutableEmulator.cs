using System.Reflection;
using Snowflake.Plugin;
using Snowflake.Plugin.Interface;

namespace Snowflake.Emulator
{
    public abstract class ExecutableEmulator : BasePlugin, IEmulator
    {
        public ExecutableEmulator(Assembly pluginAssembly):base(pluginAssembly)
        {
            //todo backport functionality from retroarch
        }
        public abstract void Run(string gameUuid);
   }
}


