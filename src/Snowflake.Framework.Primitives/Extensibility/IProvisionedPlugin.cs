using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility
{
    public interface IProvisionedPlugin : IPlugin
    {
        /// <summary>
        /// The plugin provision from the active plugin manager for this instance
        /// </summary>
        IPluginProvision Provision { get; }
    }
}
