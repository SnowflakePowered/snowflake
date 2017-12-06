using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Extensibility
{
    public abstract class StandalonePlugin : IPlugin
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Author { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public Version Version { get; }

        protected StandalonePlugin()
        {
            var attribute = this.GetType().GetCustomAttribute<PluginAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException("Plugin is not marked with an attribute");
            }

            this.Name = attribute.PluginName;
            this.Author = attribute.Author;
            this.Description = attribute.Description;
            this.Version = attribute.Version;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StandalonePlugin() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
