using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration;
using Snowflake.Controller;
using Snowflake.Service;
using Snowflake.Game;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Snowflake.Emulator.Input;
using Snowflake.Plugin;
using Snowflake.Platform;
using System.Reflection;

namespace Snowflake.Emulator
{
    public abstract class EmulatorBridge : BasePlugin, IEmulatorBridge
    {
        public IDictionary<string, IControllerTemplate> ControllerTemplates { get; private set; }
        public IDictionary<string, IInputTemplate> InputTemplates { get; private set; }
        public IDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; private set; }
        public IList<string> SupportedPlatforms { get; private set; }
        public IEmulatorAssembly EmulatorAssembly { get; private set; }

        public EmulatorBridge(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance) { }

        public abstract void StartRom(IGameInfo gameInfo);
      /*  {
            
            var retroArch = CoreService.LoadedCore.EmulatorManager.EmulatorAssemblies["retroarch"];
            string path = CoreService.LoadedCore.EmulatorManager.GetAssemblyDirectory(retroArch);
            var startInfo = new ProcessStartInfo(path);
            startInfo.WorkingDirectory = Path.Combine(CoreService.LoadedCore.EmulatorManager.AssembliesLocation, "retroarch");
            startInfo.Arguments = String.Format(@"{0} --libretro ""cores/bsnes_balanced_libretro.dll"" --config retroarch.cfg.clean --appendconfig controller.cfg", gameInfo.FileName);
            Console.WriteLine(startInfo.Arguments);
            var platform = CoreService.LoadedCore.LoadedPlatforms[gameInfo.PlatformId];
            File.WriteAllText("controller.cfg", CompileController(1, platform.Controllers[CoreService.LoadedCore.ControllerPortsDatabase.GetPort(platform, 1)], this.ControllerTemplates["NES_CONTROLLER"], profile, this.InputTemplates["retroarch"]));
            Process.Start(startInfo).WaitForExit();
           //todo needs a place to output configurations
            //configurationflags please
        }*/
        public abstract void HandlePrompt(string messagge);
        public abstract void ShutdownEmulator();
        public virtual string CompileConfiguration(IConfigurationProfile configProfile)
        {
            return this.CompileConfiguration(this.ConfigurationTemplates[configProfile.TemplateID], configProfile);
        }
        public virtual string CompileConfiguration(IConfigurationTemplate configTemplate, IConfigurationProfile configProfile)
        {
            var template = new StringBuilder(configTemplate.StringTemplate);
            foreach (var configurationValue in configProfile.ConfigurationValues)
            {
                Type configurationvalueType = configurationValue.Value.GetType();
                string stringValue;
                if (configurationvalueType == typeof(bool))
                {
                    stringValue = configTemplate.BooleanMapping.FromBool(configurationValue.Value);
                }
                else
                {
                    stringValue = configurationValue.Value.ToString();
                }
                template.Replace("{" + configurationValue.Key + "}", stringValue);
            }
            return template.ToString();
        }
        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate)
        {
            string controllerId = this.CoreInstance.ControllerPortsDatabase.GetPort(platformInfo, playerIndex);
            IControllerProfile controllerProfile = this.CoreInstance.ControllerProfileDatabase.GetControllerProfile(controllerId, playerIndex);

            return this.CompileController(playerIndex, 
                platformInfo.Controllers[controllerProfile.ControllerID],
                this.ControllerTemplates[controllerProfile.ControllerID],
                controllerProfile,
                inputTemplate);
        }
        public virtual string CompileController(int playerIndex, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IControllerProfile controllerProfile, IInputTemplate inputTemplate)
        {
            var template = new StringBuilder(inputTemplate.StringTemplate);
            var controllerMappings = controllerProfile.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                controllerTemplate.KeyboardControllerMappings : controllerTemplate.GamepadControllerMappings;

            foreach (IControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                string templateKey = controllerMappings["default"].InputMappings[input.InputName];
                string inputSetting = controllerProfile.InputConfiguration[input.InputName];
                string emulatorValue = controllerProfile.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                    inputTemplate.KeyboardMappings.First().Value[inputSetting] : inputTemplate.GamepadMappings.First().Value[inputSetting]; 
                template.Replace("{" + templateKey + "}", emulatorValue);
            }

            foreach (var key in inputTemplate.TemplateKeys)
            {
                template.Replace("{N}", playerIndex.ToString()); //Player Index
                if (controllerMappings["default"].KeyMappings.ContainsKey(key))
                {
                    template.Replace("{" + key + "}", controllerMappings["default"].KeyMappings[key]); //Non-input keys
                }
                else
                {
                    template.Replace("{" + key + "}", inputTemplate.NoBind); //Non-input keys
                }
            }
            return template.ToString();
        }
    }
}
