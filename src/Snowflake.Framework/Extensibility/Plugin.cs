using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility.Configuration;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    public abstract class Plugin : IPlugin
    {
        public string PluginName { get; }
        public IPluginProvision Provision { get; }

        /// <summary>
        /// The logger provided for this plugin
        /// </summary>
        protected ILogger Logger { get; private set; }

        protected Plugin(IPluginProvision provision)
        {
            this.PluginName = provision.Name;
            this.Provision = provision;
        }
        
        private string GetPluginName()
        {
            return this.GetType().GetTypeInfo().GetCustomAttribute<PluginAttribute>().PluginName;
        }

        public virtual void Dispose()
        {

        }
    }
}
