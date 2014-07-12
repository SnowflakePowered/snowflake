using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Snowflake.API.Interface.Core;
using Snowflake.API.Interface;

using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Snowflake.Core
{
    public class PluginManager : IPluginManager
    {
        private string AppDataDirectory { get; set; }

        [ImportMany(typeof(IIdentifier))]
        IEnumerable<Lazy<IIdentifier>> identifiers;
        [ImportMany(typeof(IEmulator))]
        IEnumerable<Lazy<IEmulator>> emulators;
        [ImportMany(typeof(IScraper))]
        IEnumerable<Lazy<IScraper>> scrapers;
        [ImportMany(typeof(IGenericPlugin))]
        IEnumerable<Lazy<IGenericPlugin>> plugins;


        public IDictionary<string, IIdentifier> LoadedIdentifiers { get; private set; }
        public IDictionary<string, IEmulator> LoadedEmulators { get; private set; }
        public IDictionary<string, IScraper> LoadedScrapers { get; private set; }
        public IDictionary<string, IGenericPlugin> LoadedPlugins { get; private set; }
        public IDictionary<string, Type> PluginRegistry { get; private set; }
        public PluginManager(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            this.PluginRegistry = new Dictionary<string, Type>();
            this.ComposeImports();
        }

        public void LoadAllPlugins()
        {
            this.LoadedIdentifiers = this.LoadPlugin<IIdentifier>(this.identifiers);
            this.LoadedEmulators = this.LoadPlugin<IEmulator>(this.emulators);
            this.LoadedScrapers = this.LoadPlugin<IScraper>(this.scrapers);
            this.LoadedPlugins = this.LoadPlugin<IGenericPlugin>(this.plugins);
        }
        private void ComposeImports()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(Path.Combine(this.AppDataDirectory, "plugins"));
            CompositionContainer container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        private Dictionary<string, T> LoadPlugin<T>(IEnumerable<Lazy<T>> plugins)
        {
            if (!(typeof(IPlugin).IsAssignableFrom(typeof(T))))
            {
                throw new ArgumentException("Attemped to load plugin that is not inherited from IPlugin");
            }
            var loadedPlugins = new Dictionary<string, T>();
           
            foreach (var plugin in plugins)
            {
                var instance = (IPlugin)plugin.Value;
                loadedPlugins.Add(instance.PluginName, plugin.Value);
                this.PluginRegistry.Add(instance.PluginName, typeof(T));

            }
            return loadedPlugins;
        }
    }
}
