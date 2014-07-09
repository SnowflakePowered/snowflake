using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Information;
using Snowflake.API.Interface;
using Snowflake.API.Database;
using Snowflake.Core.Theme;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;

namespace Snowflake.Core
{
    public class FrontendCore
    {
        public Dictionary<string, Platform> LoadedPlatforms { get; private set; }
        public string AppDataDirectory { get; private set; }

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
        public GameDatabase GameDatabase { get; private set; }
     
        [Import(typeof(IIdentifier))]
        public IIdentifier datIdentifier;

        public FrontendCore() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public FrontendCore(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
            this.ComposeImports();
            this.LoadedIdentifiers = this.LoadPlugin<IIdentifier>(this.identifiers);
            this.LoadedEmulators = this.LoadPlugin<IEmulator>(this.emulators);
            this.LoadedScrapers = this.LoadPlugin<IScraper>(this.scrapers);
            this.LoadedPlugins = this.LoadPlugin<IGenericPlugin>(this.plugins);
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));
            
            ThemeServer server = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            server.StartServer();

            Console.WriteLine(this.LoadedIdentifiers["Snowflake-IdentifierDat"].PluginName);

        }

        private Dictionary<string, Platform> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, Platform>();

            foreach (string fileName in Directory.GetFiles(platformDirectory))
            {
                if (Path.GetExtension(fileName) == ".platform")
                {
                    try
                    {
                        var platform = JsonConvert.DeserializeObject<Platform>(File.ReadAllText(fileName));
                        loadedPlatforms.Add(platform.PlatformId, platform);
                    }
                    catch (Exception)
                    {
                        //log
                        Console.WriteLine("Exception occured when importing platform " + fileName);
                        continue;
                    }
                }
            }
            return loadedPlatforms;

        }

        private void ComposeImports()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(Path.Combine(this.AppDataDirectory, "plugins"));
            CompositionContainer container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        private Dictionary<string, T> LoadPlugin<T>(IEnumerable<Lazy<T>> plugins)
        {
            if(!(typeof(IPlugin).IsAssignableFrom(typeof(T)))){
                throw new ArgumentException("Attemped to load plugin that is not inherited from IPlugin");
            }
            var loadedPlugins = new Dictionary<string, T>();
            foreach (var plugin in plugins)
            {
                var instance = (IPlugin) plugin.Value;
                loadedPlugins.Add(instance.PluginName, plugin.Value);
            }
            return loadedPlugins;
        }
    }
}
