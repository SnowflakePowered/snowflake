using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    public abstract class StandalonePlugin : ProvisionedPlugin
    {
        protected StandalonePlugin(Type pluginType)
            : base(new StandalonePluginProvision(pluginType))
        {
        }
    }
}
