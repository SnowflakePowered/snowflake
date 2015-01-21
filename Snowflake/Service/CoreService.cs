using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Service.HttpServer;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using Snowflake.Events.CoreEvents;
using System.Threading;
using Snowflake.Information;
using System.Reflection;
using Snowflake.Service.Manager;
using Snowflake.Controller;
using Snowflake.Game;
using Snowflake.Emulator.Configuration;
namespace Snowflake.Service
{
    [Export(typeof(ICoreService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CoreService : ICoreService 
    {
        #region Loaded Objects
        public IDictionary<string, IPlatformInfo> LoadedPlatforms { get; private set; }
        public IPluginManager PluginManager { get; private set; }
        public IAjaxManager AjaxManager { get; private set; }
        public IGameDatabase GameDatabase { get; private set; }
        public IControllerProfileDatabase ControllerProfileDatabase { get; private set; }
        public IControllerPortsDatabase ControllerPortsDatabase { get; private set; }
        public IConfigurationFlagDatabase ConfigurationFlagDatabase { get; private set; }
        public IPlatformPreferenceDatabase PlatformPreferenceDatabase { get; private set; }
        public IEmulatorAssembliesManager EmulatorManager { get; private set; }
        #endregion

        public string AppDataDirectory { get; private set; }
        public static ICoreService LoadedCore { get; private set; }
        public IBaseHttpServer ThemeServer { get; private set; }
        public IBaseHttpServer APIServer { get; private set; }
        public IBaseHttpServer MediaStoreServer { get; private set; }


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
      //      CoreService.LoadedCore.OnPluginManagerLoaded(new PluginManagerLoadedEventArgs());
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
            this.ControllerProfileDatabase = new ControllerProfileDatabase(Path.Combine(this.AppDataDirectory, "controllers.db"));
            this.PlatformPreferenceDatabase = new PlatformPreferencesDatabase(Path.Combine(this.AppDataDirectory, "platformprefs.db"));
            this.ControllerPortsDatabase = new ControllerPortsDatabase(Path.Combine(this.AppDataDirectory, "ports.db"));
            this.ConfigurationFlagDatabase = new ConfigurationFlagDatabase(Path.Combine(this.AppDataDirectory, "flags.db"));
            foreach (PlatformInfo platform in this.LoadedPlatforms.Values)
            {
                this.ControllerProfileDatabase.AddPlatform(platform);
                this.ControllerPortsDatabase.AddPlatform(platform);
                this.PlatformPreferenceDatabase.AddPlatform(platform);
            }
            this.PluginManager = new PluginManager(this.AppDataDirectory);
            this.AjaxManager = new AjaxManager(this.AppDataDirectory);
            this.EmulatorManager = new EmulatorAssembliesManager(Path.Combine(this.AppDataDirectory, "emulators"));

            this.ThemeServer = new ThemeServer(Path.Combine(this.AppDataDirectory, "theme"));
            this.APIServer = new ApiServer();
            this.MediaStoreServer = new FileMediaStoreServer(Path.Combine(this.AppDataDirectory, "mediastores"));
        }
        private IDictionary<string, IPlatformInfo> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, IPlatformInfo>();

            foreach (string fileName in Directory.GetFiles(platformDirectory).Where(fileName => Path.GetExtension(fileName) == ".platform"))
            {
                
                    var _platform = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName));
                    var platform = PlatformInfo.FromJsonProtoTemplate(_platform); //Convert MediaStoreKey reference to full MediaStore object
                    loadedPlatforms.Add(platform.PlatformId, platform);
               
               /* catch (Exception)
                {
                    //log
                    Console.WriteLine("Exception occured when importing platform " + fileName);
                    continue;
                }*/
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
