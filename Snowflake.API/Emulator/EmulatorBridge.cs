using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input.Template;
using Snowflake.Platform.Controller;
using Snowflake.Core;
using Snowflake.Game;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Snowflake.Extensions;


namespace Snowflake.Emulator
{
    public class EmulatorBridge
    {
        public IReadOnlyDictionary<string, ControllerTemplate> ControllerTemplates { get; private set; }
        public IReadOnlyDictionary<string, InputTemplate> InputTemplates { get; private set; }
        public IReadOnlyDictionary<string, ConfigurationTemplate> ConfigurationTemplates { get; private set; }
        public IReadOnlyList<string> SupportedPlatforms { get; private set; }

        public EmulatorBridge(IDictionary<string, ControllerTemplate> controllerTemplates, IDictionary<string, InputTemplate> inputTemplates, IDictionary<string, ConfigurationTemplate> configurationTemplates, IList<string> supportedPlatforms)
        {
            this.ControllerTemplates = controllerTemplates.AsReadOnly();
            this.InputTemplates = inputTemplates.AsReadOnly();
            this.ConfigurationTemplates = configurationTemplates.AsReadOnly();
            this.SupportedPlatforms = supportedPlatforms.AsReadOnly();
        }
           

        public void StartRom(GameInfo gameInfo)
        {
            var retroArch = FrontendCore.LoadedCore.EmulatorManager.EmulatorAssemblies["retroarch"];
            string path = FrontendCore.LoadedCore.EmulatorManager.GetAssemblyDirectory(retroArch);
            var startInfo = new ProcessStartInfo(path);
            startInfo.WorkingDirectory = Path.Combine(FrontendCore.LoadedCore.EmulatorManager.AssembliesLocation, "retroarch");
            startInfo.Arguments = String.Format(@"{0} --libretro ""cores/bsnes_balanced_libretro.dll""", gameInfo.FileName);
            Console.WriteLine(startInfo.Arguments);
            Process.Start(startInfo).WaitForExit();
        }
        
        public string CompileConfiguration(ConfigurationTemplate configTemplate, ConfigurationProfile configProfile)
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
        public string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile, InputTemplate inputTemplate)
        {
            var template = new StringBuilder(inputTemplate.StringTemplate);
            var controllerMappings = controllerProfile.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                controllerTemplate.KeyboardControllerMappings : controllerTemplate.GamepadControllerMappings;

            foreach (ControllerInput input in controllerDefinition.ControllerInputs.Values)
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
