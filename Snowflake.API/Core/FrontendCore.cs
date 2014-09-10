using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.Platform;
using Snowflake.Database;
using Snowflake.Core.Interface;
using Snowflake.Core.Server;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.CoreEvents;
using System.Threading;
using Snowflake.Core.EventDelegate;
using Snowflake.Information;
namespace Snowflake.Core
{
    public partial class FrontendCore : IFrontendCore
    {
        #region Loaded Objects
        public IDictionary<string, Platform> LoadedPlatforms { get; private set; }
        public IPluginManager PluginManager { get; private set; }
        public GameDatabase GameDatabase { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static FrontendCore LoadedCore { get; private set; }
        public ThemeServer ThemeServer { get; private set; }    
        public ApiServer APIServer { get; private set; }
       // public ResourceServer ResourceServer { get; private set; }


        #region Events
        public delegate void PluginManagerLoadedEvent(object sender, PluginManagerLoadedEventArgs e);
        public event EventHandler CoreLoaded;
        #endregion

        public static void InitCore()
        {
                var core = new FrontendCore();
                FrontendCore.LoadedCore = core;
                FrontendCore.LoadedCore.ThemeServer.StartServer();
                FrontendCore.LoadedCore.APIServer.StartServer();
        }
      
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => InitPluginManager());
        }

        public static void InitPluginManager()
        {
            FrontendCore.LoadedCore.PluginManager.LoadAllPlugins();
            FrontendCore.LoadedCore.OnPluginManagerLoaded(new PluginManagerLoadedEventArgs());
        }

        public FrontendCore() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public FrontendCore(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
         
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));
            this.PluginManager = new PluginManager(this.AppDataDirectory);

            this.ThemeServer = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            this.APIServer = new ApiServer();
          //  this.ResourceServer = new ResourceServer(this.AppDataDirectory);
            //new JsonRPCEventDelegate(3333).Notify("test", new Dictionary<string, string>() { {"test","test"}});
        }
        private Dictionary<string, Platform> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, Platform>();

            foreach (string fileName in Directory.GetFiles(platformDirectory).Where(fileName => Path.GetExtension(fileName) == ".platform"))
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
            return loadedPlatforms;

        }

        protected virtual void OnPluginManagerLoaded(PluginManagerLoadedEventArgs e)
        {
            if (CoreLoaded != null)
                CoreLoaded(this, e);
        }

    }
}
