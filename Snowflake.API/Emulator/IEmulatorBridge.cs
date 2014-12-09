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
        void StartRom(string gameId);
        void StartRom(string platformId, string gameId);
        void StartRom(string platformId, GameInfo gameInfo, ConfigurationProfile configurationProfile, IList<ControllerProfile> controllerProfiles);
        string CompileConfiguration(ConfigurationTemplate configurationTemplate);
        string CompileConfiguration(ConfigurationTemplate configurationTemplate, ConfigurationProfile configurationProfile = null);
        string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile, InputTemplate inputTemplate);
        string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controlelrTemplate, ControllerProfile controllerProfile);
        void PlaceConfigurationFiles(IDictionary<string, string> compiledConfiguration, IList<string> compiledController);
        void ShutdownEmulator();
        void HandlePrompt(string promptMessage);
        event EventHandler OnEmulatorShutdown;
        event EventHandler OnEmulatorStartup;
        event EventHandler OnEmulatorLoseFocus;
        event EventHandler OnBridgePrompt;

    }
}
