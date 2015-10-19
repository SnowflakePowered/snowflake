using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Controller;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Plugin;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Emulator
{
    public abstract class EmulatorBridge : BasePlugin, IEmulatorBridge
    {
        public IDictionary<string, IControllerTemplate> ControllerTemplates { get; }
        public IDictionary<string, IInputTemplate> InputTemplates { get; }
        public IDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; }
        public IDictionary<string, IConfigurationFlag> ConfigurationFlags { get; }
        public IConfigurationFlagStore ConfigurationFlagStore { get; }
        public IEmulatorAssembly EmulatorAssembly { get; }

        protected EmulatorBridge(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance) {

            var flagsProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("flags.json"));
            this.ConfigurationFlags = flagsProtoTemplates.Select(protoTemplate => ConfigurationFlag.FromJsonProtoTemplate(protoTemplate)).ToDictionary(key => key.Key, key => key);
            var configurationProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("configurations.json"));
            this.ConfigurationTemplates = configurationProtoTemplates.Select(protoTemplate => ConfigurationTemplate.FromJsonProtoTemplate(protoTemplate, this)).ToDictionary(key => key.TemplateID, key => key);
            var inputProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("input.json"));
            this.InputTemplates = inputProtoTemplates.Select(protoTemplate => InputTemplate.FromJsonProtoTemplate(protoTemplate, this)).ToDictionary(key => key.Name, key => key);
            var controllerProtoTemplates = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(this.GetStringResource("controllers.json"));
            this.ControllerTemplates = controllerProtoTemplates.Select(protoTemplate => ControllerTemplate.FromJsonProtoTemplate(protoTemplate)).ToDictionary(key => key.ControllerID, key => key);
            this.EmulatorAssembly = coreInstance.Get<IEmulatorAssembliesManager>().EmulatorAssemblies[this.PluginInfo["emulator_assembly"]];
            this.ConfigurationFlagStore = new ConfigurationFlagStore(this);
        }

        public abstract void StartRom(IGameInfo gameInfo);
        public abstract void HandlePrompt(string messagge);
        public abstract void ShutdownEmulator();
        public virtual string CompileConfiguration(IConfigurationProfile configProfile, IGameInfo gameInfo)
        {
            return this.CompileConfiguration(this.ConfigurationTemplates[configProfile.TemplateID], configProfile, gameInfo);
        }
        public virtual string CompileConfiguration(IConfigurationTemplate configTemplate, IConfigurationProfile configProfile, IGameInfo gameInfo)
        {
            var template = new StringBuilder(configTemplate.StringTemplate);
            foreach (var configurationValue in configProfile.ConfigurationValues)
            {
                Type configurationvalueType = configurationValue.Value.GetType();
                string stringValue = configurationvalueType == typeof(bool) ? configTemplate.BooleanMapping.FromBool(configurationValue.Value) : configurationValue.Value.ToString();
                template.Replace($"{{{configurationValue.Key}}}", stringValue);
            }
            return template.ToString();
        }
        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate, IGameInfo gameInfo)
        {
            string deviceName = this.CoreInstance.Get<IControllerPortsDatabase>().GetDeviceInPort(platformInfo, playerIndex);
            string controllerId = platformInfo.ControllerPorts[playerIndex];
            IControllerDefinition controllerDefinition = this.CoreInstance.Controllers[controllerId];
            IGamepadAbstraction gamepadAbstraction = this.CoreInstance.Get<IGamepadAbstractionDatabase>()[deviceName];
            return this.CompileController(playerIndex, 
                platformInfo,
                controllerDefinition,
                this.ControllerTemplates[controllerId],
                gamepadAbstraction,
                inputTemplate,
                gameInfo);
        }

        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IGameInfo gameInfo)
        {
            if (gamepadAbstraction.ProfileType == ControllerProfileType.NULL_PROFILE) return string.Empty;
            var controllerMappings = gamepadAbstraction.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ?
                controllerTemplate.KeyboardControllerMappings : controllerTemplate.GamepadControllerMappings;
            return this.CompileController(playerIndex, platformInfo, controllerDefinition, controllerTemplate, gamepadAbstraction, inputTemplate, controllerMappings, gameInfo);
        }

        public virtual string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IReadOnlyDictionary<string, IControllerMapping> controllerMappings, IGameInfo gameInfo)
        {
            if (gamepadAbstraction.ProfileType == ControllerProfileType.NULL_PROFILE) return string.Empty;
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
