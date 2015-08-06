using System.Reflection;
using Snowflake.Service;

namespace Snowflake.Plugin
{
    public class GeneralPlugin : BasePlugin, IGeneralPlugin
    {
        protected GeneralPlugin(Assembly pluginAssembly, ICoreService coreInstance)
            : base(pluginAssembly, coreInstance)
        {
    }
    }
}
