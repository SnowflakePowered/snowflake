using System.ComponentModel.Composition;
using Snowflake.Game;
using Snowflake.Plugin;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Controller;
using System.Collections.Generic;
using System;
using Snowflake.Platform;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulatorBridge))]
    public interface IEmulatorBridge : IBasePlugin
    {
        IEmulatorAssembly EmulatorAssembly { get; }
        IDictionary<string, IControllerTemplate> ControllerTemplates { get; }
        IDictionary<string, IInputTemplate> InputTemplates { get; }
        IDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; }
        void StartRom(IGameInfo gameInfo);
        string CompileConfiguration(IConfigurationProfile configurationProfile);
        string CompileConfiguration(IConfigurationTemplate configurationTemplate, IConfigurationProfile configurationProfile);
        string CompileController(int playerIndex, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IControllerProfile controllerProfile, IInputTemplate inputTemplate);
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate);
        void ShutdownEmulator();
        void HandlePrompt(string promptMessage);
       
    }
}
