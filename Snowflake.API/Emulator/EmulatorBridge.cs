using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input.Template;
using Snowflake.Platform.Controller;


namespace Snowflake.Emulator
{
    public class EmulatorBridge
    {
        public string CompileConfiguration(ConfigurationTemplate template, ConfigurationProfile profile)
        {
            return String.Empty;
        }

       
        public string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile, InputTemplate inputTemplate)
        {
            StringBuilder template = new StringBuilder(inputTemplate.StringTemplate);

            foreach (ControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                string templateKey = controllerTemplate.GamepadControllerMappings["default"].InputMappings[input.InputName];
                string inputSetting = controllerProfile.InputConfiguration[input.InputName];
                string emulatorValue = inputTemplate.GamepadMappings.First().Value[inputSetting];
                template.Replace("{" + templateKey + "}", emulatorValue);
            }
            foreach (var key in inputTemplate.TemplateKeys)
            {
                template.Replace("{N}", playerIndex.ToString()); //Player Index
                if (controllerTemplate.GamepadControllerMappings["default"].KeyMappings.ContainsKey(key))
                {
                    template.Replace("{" + key + "}", controllerTemplate.GamepadControllerMappings["default"].KeyMappings[key]); //Non-input keys
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
