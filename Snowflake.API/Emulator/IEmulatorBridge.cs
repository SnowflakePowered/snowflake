using System.ComponentModel.Composition;
using Snowflake.Game;
using Snowflake.Plugin;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Controller;
using System.Collections.Generic;
using System;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulatorBridge))]
    public interface IEmulatorBridge : IBasePlugin
    {
        IEmulatorAssembly EmulatorAssembly { get; }
        IReadOnlyDictionary<string, string> SupportedPlatforms { get;  }
        void StartRom(IGameInfo gameInfo);
        string CompileConfiguration(IConfigurationProfile configurationProfile);
        string CompileConfiguration(IConfigurationTemplate configurationTemplate, IConfigurationProfile configurationProfile);
        string CompileController(int playerIndex, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IControllerProfile controllerProfile);
        string CompileController(int playerIndex, IControllerProfile controllerProfile);
        void PlaceConfigurationFiles(IDictionary<string, string> compiledConfiguration, IList<string> compiledController);
        void ShutdownEmulator();
        void HandlePrompt(string promptMessage);
       
    }
}
