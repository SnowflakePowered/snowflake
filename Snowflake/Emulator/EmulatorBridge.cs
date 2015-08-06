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
using Newtonsoft.Json;

namespace Snowflake.Emulator
{
    public abstract class EmulatorBridge : BasePlugin, IEmulatorBridge
    {
        public IDictionary<string, IControllerTemplate> ControllerTemplates { get; private set; }
        public IDictionary<string, IInputTemplate> InputTemplates { get; private set; }
        public IDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; private set; }
        public IDictionary<string, IConfigurationFlag> ConfigurationFlags { get; private set; }
        public IConfigurationFlagStore ConfigurationFlagStore { get; private set; }
        public IEmulatorAssembly EmulatorAssembly { get; private set; }

        public EmulatorBridge(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance) {

            var flagsProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("flags.json"));
            this.ConfigurationFlags = flagsProtoTemplates.Select(protoTemplate => ConfigurationFlag.FromJsonProtoTemplate(protoTemplate)).ToDictionary(key => key.Key, key => key);
            var configurationProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("configurations.json"));
            this.ConfigurationTemplates = configurationProtoTemplates.Select(protoTemplate => ConfigurationTemplate.FromJsonProtoTemplate(protoTemplate, this)).ToDictionary(key => key.TemplateID, key => key);
            var inputProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("input.json"));
            this.InputTemplates = inputProtoTemplates.Select(protoTemplate => InputTemplate.FromJsonProtoTemplate(protoTemplate, this)).ToDictionary(key => key.Name, key => key);
            var controllerProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("controllers.json"));
            this.ControllerTemplates = controllerProtoTemplates.Select(protoTemplate => ControllerTemplate.FromJsonProtoTemplate(protoTemplate)).ToDictionary(key => key.ControllerID, key => key);
            this.EmulatorAssembly = coreInstance.EmulatorManager.EmulatorAssemblies[this.PluginInfo["emulator_assembly"]];
            this.ConfigurationFlagStore = new ConfigurationFlagStore(this);
        }

        public abstract void StartRom(IGameInfo gameInfo);
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
                template.Replace($"{{{configurationValue.Key}}}", stringValue);
            }
            return template.ToString();
        }
        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate)
        {
            string deviceName = this.CoreInstance.ControllerPortsDatabase.GetDeviceInPort(platformInfo, playerIndex);
            string controllerId = platformInfo.ControllerPorts[playerIndex];
            IControllerDefinition controllerDefinition = this.CoreInstance.LoadedControllers[controllerId];
            IGamepadAbstraction gamepadAbstraction = this.CoreInstance.GamepadAbstractionDatabase[deviceName];
            return this.CompileController(playerIndex, 
                platformInfo,
                controllerDefinition,
                this.ControllerTemplates[controllerId],
                gamepadAbstraction,
                inputTemplate);
        }

        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate)
        {
            if (gamepadAbstraction.ProfileType == ControllerProfileType.NULL_PROFILE) return String.Empty;
            var controllerMappings = gamepadAbstraction.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ?
                controllerTemplate.KeyboardControllerMappings : controllerTemplate.GamepadControllerMappings;
            return this.CompileController(playerIndex, platformInfo, controllerDefinition, controllerTemplate, gamepadAbstraction, inputTemplate, controllerMappings);
        }

        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IReadOnlyDictionary<string, IControllerMapping> controllerMappings)
        {
            if (gamepadAbstraction.ProfileType == ControllerProfileType.NULL_PROFILE) return String.Empty;
            var template = new StringBuilder(inputTemplate.StringTemplate);
            foreach (IControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                string templateKey = controllerMappings["default"].InputMappings[input.InputName];
                string inputSetting = gamepadAbstraction[input.GamepadAbstraction];
                string emulatorValue = gamepadAbstraction.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                    inputTemplate.KeyboardMappings.First().Value[inputSetting] : inputTemplate.GamepadMappings.First().Value[inputSetting]; 
                template.Replace($"{{{templateKey}}}", emulatorValue);
            }

            foreach (var key in inputTemplate.TemplateKeys)
            {
                template.Replace("{N}", playerIndex.ToString()); //Player Index
                if (controllerMappings["default"].KeyMappings.ContainsKey(key))
                {
                    template.Replace($"{{{key}}}", controllerMappings["default"].KeyMappings[key]); //Non-input keys
                }
                else
                {
                    template.Replace($"{{{key}}}", inputTemplate.NoBind); //Non-input keys
                }
            }
            return template.ToString();
        }
    }
}
