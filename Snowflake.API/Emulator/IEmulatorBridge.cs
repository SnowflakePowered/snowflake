using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Controller;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Plugin;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents an Emulator Bridge
    /// An Emulator Bridge 'bridges' the gap between a wrapper assembly, such as an .exe or a libretro core and
    /// wraps it so that Snowflake can call into it using specified endpoints. It is responsible for managing
    /// the controller and emulator configuration for the emulator, converting from Snowflake's config format into 
    /// whatever format the assembly accepts.
    /// </summary>
    [InheritedExport(typeof(IEmulatorBridge))]
    public interface IEmulatorBridge : IBasePlugin
    {
        /// <summary>
        /// The emulator assembly this plugin bridges to
        /// </summary>
        IEmulatorAssembly EmulatorAssembly { get; }
        /// <summary>
        /// The controller templates for this emulator bridge
        /// </summary>
        IDictionary<string, IControllerTemplate> ControllerTemplates { get; }
        /// <summary>
        /// The input templates for this emulator bridge
        /// </summary>
        IDictionary<string, IInputTemplate> InputTemplates { get; }
        /// <summary>
        /// The configuration templates for this emulator bridge
        /// </summary>
        IDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; }
        /// <summary>
        /// A set of configuration flags
        /// </summary>
        IDictionary<string, IConfigurationFlag> ConfigurationFlags { get; }
        /// <summary>
        /// A configuration flag store for this emulator bridge
        /// </summary>
        IConfigurationFlagStore ConfigurationFlagStore { get; }
        /// <summary>
        /// Start a game rom
        /// </summary>
        /// <param name="gameInfo">The game to start</param>
        void StartRom(IGameInfo gameInfo);
        /// <summary>
        /// Compile configuration
        /// </summary>
        /// <param name="configurationProfile">The configuration profile to compile</param>
        /// <returns>The compiled configuration</returns>
        string CompileConfiguration(IConfigurationProfile configurationProfile, IGameInfo gameInfo);
        /// <summary>
        /// Compile configuration
        /// </summary>
        /// <param name="configurationTemplate">The configuration template for the profile</param>
        /// <param name="configurationProfile">The configuration profile to compile</param>
        /// <returns></returns>
        
        string CompileConfiguration(IConfigurationTemplate configurationTemplate, IConfigurationProfile configurationProfile, IGameInfo game);
        /// <summary>
        /// Compile the controller for a specified player index
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="controllerDefinition">The controller definition to compile for</param>
        /// <param name="controllerTemplate">The controller template to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <returns></returns>
        /// 
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IGameInfo game);
        /// <summary>
        /// Compile a controller given a specific platform
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="platformInfo">The platform to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <returns></returns>
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate, IGameInfo game);

        /// <summary>
        /// Compile Controller given all available information
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="platformInfo">The platform to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <param name="controllerDefinition"></param>
        /// <param name="controllerTemplate"></param>
        /// <param name="gamepadAbstraction"></param>
        /// <param name="controllerMappings"></param>
        /// <returns></returns>
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IReadOnlyDictionary<string, IControllerMapping> controllerMappings, IGameInfo game);

        /// <summary>
        /// Shutdownt the currently running emulator
        /// </summary>
        void ShutdownEmulator();
        /// <summary>
        /// Handle a message sent to the emulator bridge 
        /// </summary>
        /// <param name="promptMessage">The message sent to the emulator</param>
        void HandlePrompt(string promptMessage);
       
    }
}
