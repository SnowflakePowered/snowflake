using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input.Template;
using Snowflake.Platform.Controller;

using System.Collections;


namespace Snowflake.Emulator
{
    public class EmulatorBridge
    {

        public IReadOnlyDictionary<string, ControllerTemplate> ControllerTemplates { get; private set; }
        public IReadOnlyDictionary<string, InputTemplate> InputTemplates { get; private set; }
        public IReadOnlyDictionary<string, ConfigurationTemplate> ConfigurationTemplates { get; private set; }
        public IReadOnlyList<string> SupportedPlatforms { get; private set; }

        public string CompileConfiguration(ConfigurationTemplate template, ConfigurationProfile profile)
        {
            return String.Empty;
        }

       
        public string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile, InputTemplate inputTemplate)
        {
            StringBuilder template = new StringBuilder(inputTemplate.StringTemplate);

            
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
