using System.ComponentModel.Composition;
using Snowflake.Game;
using Snowflake.Plugin;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Emulator.Input;
using Snowflake.Emulator.Input.Template;
using Snowflake.Platform.Controller;
using System.Collections.Generic;
using System;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulatorBridge))]
    public interface IEmulatorBridge : IPlugin
    {
        EmulatorAssembly EmulatorAssembly { get; }
        IReadOnlyDictionary<string, string> SupportedPlatforms { get;  }
        void StartRom(GameInfo gameInfo);
        string CompileConfiguration(ConfigurationProfile configurationProfile);
        string CompileConfiguration(ConfigurationTemplate configurationTemplate, ConfigurationProfile configurationProfile);
        string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile);
        string CompileController(int playerIndex, ControllerProfile controllerProfile);
        void PlaceConfigurationFiles(IDictionary<string, string> compiledConfiguration, IList<string> compiledController);
        void ShutdownEmulator();
        void HandlePrompt(string promptMessage);
       
    }
}
