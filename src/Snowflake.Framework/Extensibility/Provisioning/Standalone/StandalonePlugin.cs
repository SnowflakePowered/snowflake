using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    public class StandalonePlugin : ProvisionedPlugin
    {
        protected StandalonePlugin(Type pluginType)
            : base(new StandalonePluginProvision(pluginType))
        {
        }
    }
}
