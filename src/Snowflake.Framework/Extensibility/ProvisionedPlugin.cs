using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Services;

namespace Snowflake.Extensibility
{
    public abstract class ProvisionedPlugin : IProvisionedPlugin
    {
        public IPluginProvision Provision { get; }

        public string Name => this.Provision.Name;

        public string Author => this.Provision.Author;

        public string Description => this.Provision.Description;

        public Version Version => this.Provision.Version;

        protected ProvisionedPlugin(IPluginProvision provision)
        {
            this.Provision = provision;
        }
        
        private string GetPluginName()
        {
            return this.GetType().GetTypeInfo().GetCustomAttribute<PluginAttribute>().PluginName;
        }

        public virtual void Dispose()
        {

        }

        public virtual IConfigurationSection GetConfiguration()
        {
            return EmptyPluginConfiguration.EmptyConfiguration;
        }
    }
}
