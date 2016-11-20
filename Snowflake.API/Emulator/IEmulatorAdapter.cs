using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Emulator adapters manage the instance generation of a certain emulator that supports one or more
    /// Stone ROM formats. They are responsible for creating an <see cref="IEmulatorInstance"/> that will
    /// generate correect configuration given the available ports and input mappings, as well as
    /// responsible for supplying valid configuration for a given game. 
    /// </summary>
    public interface IEmulatorAdapter : IPlugin
    {
        /// <summary>
        /// The input mappings for this emulator adapter. 
        /// <para>
        /// This should be specified in JSON input mapping format, in the
        /// InputMappings directory under the plugin resource folder.
        /// </para>
        /// <para>
        /// The general convention for these files are
        /// InputApi.DEVICE_NAME
        /// </para>
        /// </summary>
        IEnumerable<IInputMapping> InputMappings { get; }
        /// <summary>
        /// A list of emulator capabiities strings under the capabilities metadata key.
        /// This key is optional, and there is no formal specification for these capabilities,
        /// which may include ingame overlays, cloud saves, etc. 
        /// </summary>
        IEnumerable<string> Capabilities { get; }
        /// <summary>
        /// A list of mimetypes this emulator can execute. This is a required key, 
        /// an emulator without supported mimetypes will not display for any
        /// file type.
        /// </summary>
        IEnumerable<string> Mimetypes { get; }
        /// <summary>
        /// A list of BIOS files this emulator requires, listed under the optional metadata key
        /// requiredbios.
        /// </summary>
        IEnumerable<string> RequiredBios { get; }
        /// <summary>
        /// A list of BIOS files this emulator supports optionally, listed under the optional metata key
        /// optionalbios.
        /// </summary>
        IEnumerable<string> OptionalBios { get; }
        /// <summary>
        /// The save type this emulator uses. Emulators with the same save type will share a directory
        /// allowing save sharing across emulators. Should your emulator support a different save type, 
        /// please change this to a unique value to avoid save conflicts.
        /// </summary>
        string SaveType { get; }
        /// <summary>
        /// The save manager for this emulator adapter, used to manage separate save files for
        /// individual games. 
        /// </summary>
        ISaveManager SaveManager { get; }
        /// <summary>
        /// The BIOS Manager for this emulator adapter, used to managed installed BIOS files.
        /// </summary>
        IBiosManager BiosManager { get; }
        /// <summary>
        /// Creates an <see cref="IEmulatorInstance"/> that can be used to spawn the emulator process and generate
        /// valid cofiguration. Instances are trusted to generate valid configuration in accordance with the 
        /// given parameters, including providing the correct save file and bios directories.
        /// 
        /// However, it is NOT the responsibility of the adapter to launch the game.
        /// </summary>
        /// <param name="gameRecord">The <see cref="IGameRecord"/> associated with this instance</param>
        /// <param name="romFile">The <see cref="IFileRecord"/> belonging to the game, that is the entry point for the emulation</param>
        /// <param name="saveSlot">The save manager slot in which this instances save files are stored</param>
        /// <param name="ports">The list of emulated controler ports active for this instance.</param>
        /// <returns>The prepared emulator instance.</returns>
        IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);
        /// <summary>
        /// Gets the valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <seealso cref="IConfigurationCollectionStore"/>
        /// <param name="gameRecord">The game that is associated with this set of configuration collections</param>
        /// <param name="profileName">The profile name to get configuration for. By default, the profile name must be 'default'/param>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default");
        /// <summary>
        /// Gets the default valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        IConfigurationCollection GetConfiguration();
    }
}
