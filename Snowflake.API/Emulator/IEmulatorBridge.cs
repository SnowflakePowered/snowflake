using System.Collections.Generic;
using Snowflake.Controller;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Extensibility;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents an Emulator Bridge
    /// An Emulator Bridge 'bridges' the gap between a wrapper assembly, such as an .exe or a libretro core and
    /// wraps it so that Snowflake can call into it using specified endpoints. It is responsible for managing
    /// the controller and emulator configuration for the emulator, converting from Snowflake's config format into 
    /// whatever format the assembly accepts.
    /// </summary>
    public interface IEmulatorBridge : IPlugin
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
        /// Prepares an emulator instance to get ready to start the game.
        /// </summary>
        /// <param name="gameInfo">The game to start</param>
        IEmulatorInstance SetupInstance(IGameInfo gameInfo);
        /// <summary>
        /// Compile configuration
        /// </summary>
        /// <param name="configurationProfile">The configuration profile to compile</param>
        /// <returns>The compiled configuration</returns>
        string CompileConfiguration(IConfigurationProfile configurationProfile, IEmulatorInstance instance);

        /// <summary>
        /// Compile configuration
        /// </summary>
        /// <param name="configurationTemplate">The configuration template for the profile</param>
        /// <param name="configurationProfile">The configuration profile to compile</param>
        /// <param name="instance">The instance this compiliation is for</param>
        /// <returns>The compiled configuration</returns>
        string CompileConfiguration(IConfigurationTemplate configurationTemplate, IConfigurationProfile configurationProfile, IEmulatorInstance instance);
        /// <summary>
        /// Compile the controller for a specified player index
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="controllerDefinition">The controller definition to compile for</param>
        /// <param name="controllerTemplate">The controller template to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <param name="instance">The instance this compiliation is for</param>
        /// <returns>The compiled controller configuration</returns>
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IEmulatorInstance instance);
        /// <summary>
        /// Compile a controller given a specific platform
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="platformInfo">The platform to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <param name="instance">The instance this compiliation is for</param>=
        /// <returns>The compiled controller configuration</returns>
        string CompileController(int playerIndex, IPlatformInfo platformInfo, IInputTemplate inputTemplate, IEmulatorInstance instance);

        /// <summary>
        /// Compile Controller given all available information
        /// </summary>
        /// <param name="playerIndex">The player index to compile for</param>
        /// <param name="platformInfo">The platform to compile for</param>
        /// <param name="inputTemplate">The input template to compile for</param>
        /// <param name="controllerDefinition">The controller definition to compile for</param>
        /// <param name="controllerTemplate">The controller template</param>
        /// <param name="gamepadAbstraction">The gamepad abstraction used</param>
        /// <param name="controllerMappings">The controller mappings used</param>
        /// <param name="instance">The instance this compiliation is for</param>
        /// <returns>The compiled controller configuration</returns>

        string CompileController(int playerIndex, IPlatformInfo platformInfo, IControllerDefinition controllerDefinition, IControllerTemplate controllerTemplate, IGamepadAbstraction gamepadAbstraction, IInputTemplate inputTemplate, IReadOnlyDictionary<string, IControllerMapping> controllerMappings, IEmulatorInstance instance);
  
    }
}
