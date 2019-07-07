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

namespace Snowflake.Extensibility.Provisioning
{
    public abstract class ProvisionedPlugin : IProvisionedPlugin
    {
        /// <inheritdoc/>
        public IPluginProvision Provision { get; }

        /// <inheritdoc/>
        public string Name => this.Provision.Name;

        /// <inheritdoc/>
        public string Author => this.Provision.Author;

        /// <inheritdoc/>
        public string Description => this.Provision.Description;

        /// <inheritdoc/>
        public Version Version => this.Provision.Version;

        protected ProvisionedPlugin(IPluginProvision provision)
        {
            this.Provision = provision;
        }

        /// <inheritdoc/>
        public virtual IConfigurationSection GetPluginConfiguration()
        {
            return EmptyPluginConfiguration.EmptyConfiguration;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
        }
    }
}
