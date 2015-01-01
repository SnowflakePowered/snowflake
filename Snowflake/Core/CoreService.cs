using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Database;
using Snowflake.Service.Manager.Interface;
using Snowflake.Service.Server;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.CoreEvents;
using System.Threading;
using Snowflake.Information;
using System.Reflection;
using Snowflake.Service.Manager;
namespace Snowflake.Service
{
    public partial class CoreService 
    {
        #region Loaded Objects
        public IDictionary<string, PlatformInfo> LoadedPlatforms { get; private set; }
        public IPluginManager PluginManager { get; private set; }
        public IAjaxManager AjaxManager { get; private set; }
        public GameDatabase GameDatabase { get; private set; }
        public ControllerDatabase ControllerDatabase { get; private set; }
        public ControllerPortsDatabase ControllerPortsDatabase { get; private set; }
        public ConfigurationFlagDatabase ConfigurationFlagDatabase { get; private set; }
        public EmulatorManager EmulatorManager { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static CoreService LoadedCore { get; private set; }
        public ThemeServer ThemeServer { get; private set; }    
        public ApiServer APIServer { get; private set; }
        public FileMediaStoreServer MediaStoreServer { get; private set; }


        #region Events
        public delegate void PluginManagerLoadedEvent(object sender, PluginManagerLoadedEventArgs e);
        public event EventHandler CoreLoaded;
        #endregion

        public static void InitCore()
        {
            var core = new CoreService();
            CoreService.LoadedCore = core;
            CoreService.LoadedCore.ThemeServer.StartServer();
            CoreService.LoadedCore.APIServer.StartServer();
            CoreService.LoadedCore.MediaStoreServer.StartServer();
        }
      
        public async static Task InitPluginManagerAsync()
        {
            await Task.Run(() => InitPluginManager());
        }

        public static void InitPluginManager()
        {
            CoreService.LoadedCore.PluginManager.LoadAll();
            CoreService.LoadedCore.AjaxManager.LoadAll();
            CoreService.LoadedCore.EmulatorManager.LoadEmulatorAssemblies();
            CoreService.LoadedCore.OnPluginManagerLoaded(new PluginManagerLoadedEventArgs());
        }

        public CoreService() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public CoreService(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
#if DEBUG
            this.AppDataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#endif
            this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));
         
            this.GameDatabase = new GameDatabase(Path.Combine(this.AppDataDirectory, "games.db"));
            this.ControllerDatabase = new ControllerDatabase(Path.Combine(this.AppDataDirectory, "controllers.db"));

            this.ControllerDatabase.LoadTables(this.LoadedPlatforms);

            this.ControllerPortsDatabase = new ControllerPortsDatabase(Path.Combine(this.AppDataDirectory, "ports.db"));
            this.ConfigurationFlagDatabase = new ConfigurationFlagDatabase(Path.Combine(this.AppDataDirectory, "flags.db"));
            foreach (PlatformInfo platform in this.LoadedPlatforms.Values)
            {
                this.ControllerPortsDatabase.AddPlatform(platform);
            }
            this.PluginManager = new PluginManager(this.AppDataDirectory);
            this.AjaxManager = new AjaxManager(this.AppDataDirectory);
            this.EmulatorManager = new EmulatorManager(Path.Combine(this.AppDataDirectory, "emulators"));

            this.ThemeServer = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            this.APIServer = new ApiServer();
            this.MediaStoreServer = new FileMediaStoreServer(Path.Combine(this.AppDataDirectory, "mediastores"));
        }
        private IDictionary<string, PlatformInfo> LoadPlatforms(string platformDirectory)
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
