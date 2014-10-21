using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Snowflake.Ajax;
using Snowflake.Core.Manager.Interface;
using Snowflake.Emulator;
using Snowflake.Plugin;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Snowflake.Scraper;

namespace Snowflake.Core.Manager
{
    public class PluginManager : IPluginManager
    {
        public string LoadablesLocation { get; private set; }
        public IDictionary<string, Type> Registry { get; private set; }

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
        public PluginManager(string loadablesLocation)
        {
            this.LoadablesLocation = loadablesLocation;
            this.Registry = new Dictionary<string, Type>();
            this.ComposeImports();
        }

        public void LoadAll()
        {
            this.LoadedIdentifiers = this.LoadPlugin(this.identifiers);
            this.LoadedEmulators = this.LoadPlugin(this.emulators);
            this.LoadedScrapers = this.LoadPlugin(this.scrapers);
            this.LoadedPlugins = this.LoadPlugin(this.plugins);
            
        }
        private void ComposeImports()
        {
            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "plugins"));
            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        private Dictionary<string, T> LoadPlugin<T>(IEnumerable<Lazy<T>> unloadedPlugins) where T : IPlugin
        {
            var loadedPlugins = new Dictionary<string, T>();
            foreach (var plugin in unloadedPlugins)
            {
                var instance = (IPlugin)plugin.Value;
                loadedPlugins.Add(instance.PluginName, plugin.Value);
                this.Registry.Add(instance.PluginName, typeof(T));
            }
            return loadedPlugins;
        }
    }
}
