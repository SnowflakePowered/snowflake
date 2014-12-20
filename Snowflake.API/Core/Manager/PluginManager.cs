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
using Snowflake.Extensions;
using Snowflake.Plugin;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Snowflake.Scraper;

namespace Snowflake.Core.Manager
{
    public class PluginManager : IPluginManager
    {
        public string LoadablesLocation { get; private set; }

        private IDictionary<string, Type> registry;
        public IReadOnlyDictionary<string, Type> Registry { get { return this.registry.AsReadOnly(); } }
        [ImportMany(typeof(IIdentifier))]
        IEnumerable<Lazy<IIdentifier>> identifiers;
        [ImportMany(typeof(IEmulatorBridge))]
        IEnumerable<Lazy<IEmulatorBridge>> emulators;
        [ImportMany(typeof(IScraper))]
        IEnumerable<Lazy<IScraper>> scrapers;
        [ImportMany(typeof(IGenericPlugin))]
        IEnumerable<Lazy<IGenericPlugin>> plugins;



        private IDictionary<string, IIdentifier> loadedIdentifiers;
        private IDictionary<string, IEmulatorBridge> loadedEmulators;
        private IDictionary<string, IScraper> loadedScrapers;
        private IDictionary<string, IGenericPlugin> loadedPlugins;

        public IReadOnlyDictionary<string, IIdentifier> LoadedIdentifiers { get { return this.loadedIdentifiers.AsReadOnly(); } }
        public IReadOnlyDictionary<string, IEmulatorBridge> LoadedEmulators { get { return this.loadedEmulators.AsReadOnly(); } }
        public IReadOnlyDictionary<string, IScraper> LoadedScrapers { get { return this.loadedScrapers.AsReadOnly(); } }
        public IReadOnlyDictionary<string, IGenericPlugin> LoadedPlugins { get { return this.loadedPlugins.AsReadOnly(); } }

        public PluginManager(string loadablesLocation)
        {
            this.LoadablesLocation = loadablesLocation;
            this.registry = new Dictionary<string, Type>();
            this.ComposeImports();
        }

        public void LoadAll()
        {
            this.loadedIdentifiers = this.LoadPlugin(this.identifiers);
            this.loadedEmulators = this.LoadPlugin(this.emulators);
            this.loadedScrapers = this.LoadPlugin(this.scrapers);
            this.loadedPlugins = this.LoadPlugin(this.plugins);
            
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
                instance.CoreInstance = FrontendCore.LoadedCore;
                loadedPlugins.Add(instance.PluginName, plugin.Value);
                this.registry.Add(instance.PluginName, typeof(T));
            }
            return loadedPlugins;
        }
    }
}
