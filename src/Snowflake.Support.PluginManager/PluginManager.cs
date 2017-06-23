using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using Snowflake.Utility;
using System.IO;

namespace Snowflake.Support.PluginManager
{
    public class PluginManager : IPluginManager
    {
        private readonly ILogProvider logProvider;
        private readonly IContentDirectoryProvider contentDirectory;
        public PluginManager(ILogProvider logProvider, IContentDirectoryProvider contentDirectory)
        {
            this.logProvider = logProvider;
            this.contentDirectory = contentDirectory;
        }

        public IPluginProvision GetProvision<T>(IModule composableModule) where T : IPlugin
        {
            var resourceDirectory = composableModule.ModuleDirectory.CreateSubdirectory("resource"); //todo: check for missing directory!!
            var pluginName = typeof(T).GetTypeInfo().GetCustomAttribute<PluginAttribute>().PluginName;
            if (pluginName == "common") throw new UnauthorizedAccessException("Plugin name can not be 'common'.");
            var pluginResourceDirectory = resourceDirectory.CreateSubdirectory(pluginName);
            var pluginCommonResourceDirectory = resourceDirectory.CreateSubdirectory("common");
            IPluginProperties properties = new JsonPluginProperties(JObject
               .FromObject(JsonConvert
               .DeserializeObject(File.ReadAllText(pluginResourceDirectory.GetFiles().Where(f => f.Name == "plugin.json")
               .First().FullName)), new JsonSerializer { Culture = CultureInfo.InvariantCulture }));
            var pluginDataDirectory = this.contentDirectory.ApplicationData.CreateSubdirectory("plugincontents")
                    .CreateSubdirectory(pluginName);
            return new PluginProvision(this.logProvider.GetLogger($"Plugin:{pluginName}"), 
                properties, pluginName, pluginDataDirectory, pluginCommonResourceDirectory, pluginResourceDirectory);
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
        // ~PluginManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public void Register<T>(T plugin) where T : IPlugin
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
