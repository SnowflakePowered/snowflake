using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Information.Platform;
using Snowflake.API.Interface;
using Snowflake.API.Database;
using Snowflake.Core.Theme;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.CoreEvents;

namespace Snowflake.Core
{
    public partial class FrontendCore : IFrontendCore
    {
        #region Loaded Objects
        public Dictionary<string, Platform> LoadedPlatforms { get; private set; }
        public PluginManager PluginManager { get; private set; }
        public GameDatabase GameDatabase { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static FrontendCore LoadedCore { get; private set; }

        #region Events
        public delegate void PluginManagerLoadedEvent(object sender, PluginManagerLoadedEventArgs e);
        public event EventHandler CoreLoaded;
        #endregion

        public static void InitCore()
        {
            var core = new FrontendCore();
            FrontendCore.LoadedCore = core;
            FrontendCore.InitPluginManagerAsync();

        }
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => FrontendCore.LoadedCore.PluginManager.LoadAllPlugins());
           
            FrontendCore.LoadedCore.OnPluginManagerLoaded(new PluginManagerLoadedEventArgs());
        }
        public FrontendCore() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public FrontendCore(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
         
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));
            this.PluginManager = new PluginManager(this.AppDataDirectory);

            ThemeServer server = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            server.StartServer();

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

        protected virtual void OnPluginManagerLoaded(PluginManagerLoadedEventArgs e)
        {
            if (CoreLoaded != null)
                CoreLoaded(this, e);
        }
    }
}
