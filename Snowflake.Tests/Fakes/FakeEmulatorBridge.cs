using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeEmulatorBridge : Snowflake.Emulator.IEmulatorBridge
    {


        public Emulator.IEmulatorAssembly EmulatorAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Emulator.Input.IControllerTemplate> ControllerTemplates
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Emulator.Input.IInputTemplate> InputTemplates
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Emulator.Configuration.IConfigurationTemplate> ConfigurationTemplates
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, Emulator.Configuration.IConfigurationFlag> ConfigurationFlags
        {
            get { throw new NotImplementedException(); }
        }

        public Emulator.Configuration.IConfigurationFlagStore ConfigurationFlagStore
        {
            get { throw new NotImplementedException(); }
        }

        public void StartRom(Game.IGameInfo gameInfo)
        {
            throw new NotImplementedException();
        }

        public string CompileConfiguration(Emulator.Configuration.IConfigurationProfile configurationProfile)
        {
            throw new NotImplementedException();
        }

        public string CompileConfiguration(Emulator.Configuration.IConfigurationTemplate configurationTemplate, Emulator.Configuration.IConfigurationProfile configurationProfile)
        {
            throw new NotImplementedException();
        }

        public string CompileController(int playerIndex, Platform.IPlatformInfo platformInfo, Controller.IControllerDefinition controllerDefinition, Emulator.Input.IControllerTemplate controllerTemplate, Controller.IGamepadAbstraction gamepadAbstraction, Emulator.Input.IInputTemplate inputTemplate)
        {
            throw new NotImplementedException();
        }

        public string CompileController(int playerIndex, Platform.IPlatformInfo platformInfo, Emulator.Input.IInputTemplate inputTemplate)
        {
            throw new NotImplementedException();
        }

        public string CompileController(int playerIndex, Platform.IPlatformInfo platformInfo, Controller.IControllerDefinition controllerDefinition, Emulator.Input.IControllerTemplate controllerTemplate, Controller.IGamepadAbstraction gamepadAbstraction, Emulator.Input.IInputTemplate inputTemplate, IReadOnlyDictionary<string, Emulator.Input.IControllerMapping> controllerMappings)
        {
            throw new NotImplementedException();
        }

        public void ShutdownEmulator()
        {
            throw new NotImplementedException();
        }

        public void HandlePrompt(string promptMessage)
        {
            throw new NotImplementedException();
        }

        public string PluginName
        {
            get { throw new NotImplementedException(); }
        }

        public string PluginDataPath
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> SupportedPlatforms
        {
            get { throw new NotImplementedException(); }
        }

        public System.Reflection.Assembly PluginAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, dynamic> PluginInfo
        {
            get { throw new NotImplementedException(); }
        }

        public Service.ICoreService CoreInstance
        {
            get { throw new NotImplementedException(); }
        }

        public System.IO.Stream GetResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public string GetStringResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public Plugin.IPluginConfiguration PluginConfiguration
        {
            get { throw new NotImplementedException(); }
        }
    }
}
