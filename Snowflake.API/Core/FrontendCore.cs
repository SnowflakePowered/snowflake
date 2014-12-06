using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Database;
using Snowflake.Core.Manager.Interface;
using Snowflake.Core.Server;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.CoreEvents;
using System.Threading;
using Snowflake.Core.EventDelegate;
using Snowflake.Information;
using System.Reflection;
using Snowflake.Core.Manager;
namespace Snowflake.Core
{
    public partial class FrontendCore 
    {
        #region Loaded Objects
        public IDictionary<string, PlatformInfo> LoadedPlatforms { get; private set; }
        public IPluginManager PluginManager { get; private set; }
        public IAjaxManager AjaxManager { get; private set; }
        public GameDatabase GameDatabase { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static FrontendCore LoadedCore { get; private set; }
        public ThemeServer ThemeServer { get; private set; }    
        public ApiServer APIServer { get; private set; }
        public FileMediaStoreServer MediaStoreServer { get; private set; }


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
            FrontendCore.LoadedCore.MediaStoreServer.StartServer();
        }
      
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => InitPluginManager());
        }

        public static void InitPluginManager()
        {
            FrontendCore.LoadedCore.PluginManager.LoadAll();
            FrontendCore.LoadedCore.AjaxManager.LoadAll();
            FrontendCore.LoadedCore.OnPluginManagerLoaded(new PluginManagerLoadedEventArgs());
        }

        public FrontendCore() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public FrontendCore(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
#if DEBUG
            this.AppDataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#endif
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
         
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));

           
            this.PluginManager = new PluginManager(this.AppDataDirectory);
            this.AjaxManager = new AjaxManager(this.AppDataDirectory);

            this.ThemeServer = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            this.APIServer = new ApiServer();
            this.MediaStoreServer = new FileMediaStoreServer(Path.Combine(this.AppDataDirectory, "mediastores"));
        }
        private Dictionary<string, PlatformInfo> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, PlatformInfo>();

            foreach (string fileName in Directory.GetFiles(platformDirectory).Where(fileName => Path.GetExtension(fileName) == ".platform"))
            {
                try
                {
                    var _platform = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName));
                    var platform = PlatformInfo.FromDictionary(_platform); //Convert MediaStoreKey reference to full MediaStore object
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
