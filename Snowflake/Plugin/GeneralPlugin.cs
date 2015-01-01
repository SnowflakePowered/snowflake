using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin;
using Snowflake.Service;
using System.Reflection;

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
