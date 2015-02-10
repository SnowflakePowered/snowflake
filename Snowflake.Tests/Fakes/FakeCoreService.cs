using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
namespace Snowflake.Tests.Fakes
{
    public class FakeCoreServices : ICoreService
    {

        public Service.Manager.IAjaxManager AjaxManager
        {
            get { throw new NotImplementedException(); }
        }

        public Service.HttpServer.IBaseHttpServer APIServer
        {
            get { throw new NotImplementedException(); }
        }

        public Service.Manager.IEmulatorAssembliesManager EmulatorManager
        {
            get { throw new NotImplementedException(); }
        }

        public string AppDataDirectory
        {
            get { throw new NotImplementedException(); }
        }

        public Controller.IControllerPortsDatabase ControllerPortsDatabase
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler CoreLoaded;

        public Game.IGameDatabase GameDatabase
        {
            get { throw new NotImplementedException(); }
        }

        public Platform.IPlatformPreferenceDatabase PlatformPreferenceDatabase
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Platform.IPlatformInfo> LoadedPlatforms
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Controller.IControllerDefinition> LoadedControllers
        {
            get { throw new NotImplementedException(); }
        }

        public Service.HttpServer.IBaseHttpServer MediaStoreServer
        {
            get { throw new NotImplementedException(); }
        }

        public Service.Manager.IPluginManager PluginManager
        {
            get { throw new NotImplementedException(); }
        }

        public Service.HttpServer.IBaseHttpServer ThemeServer
        {
            get { throw new NotImplementedException(); }
        }
    }
}
