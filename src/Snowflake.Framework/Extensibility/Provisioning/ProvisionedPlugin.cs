using System;
using Snowflake.Configuration;

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
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
