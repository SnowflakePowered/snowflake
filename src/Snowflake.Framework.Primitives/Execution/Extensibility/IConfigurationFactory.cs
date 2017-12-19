using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Extensibility
{
    /// <summary>
    /// Provides configuration collections specific to the given emulator.
    /// </summary>
    public interface IConfigurationFactory
    {
        /// <summary>
        /// Gets the input mappings for this emulator adapter.
        /// <para>
        /// This should be specified in JSON input mapping format, in the
        /// InputMappings directory under the plugin resource folder.
        /// </para>
        /// <para>
        /// The general convention for these files are
        /// common/InputMapping/DEVICE_NAME
        /// </para>
        /// </summary>
        IEnumerable<IInputMapping> InputMappings { get; }

        /// <summary>
        /// Gets the valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <seealso cref="IConfigurationCollectionStore"/>
        /// <param name="gameRecord">The game that is associated with this set of configuration collections</param>
        /// <param name="profileName">The profile name to get configuration for. By default, the profile name must be 'default'</param>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default");

        /// <summary>
        /// Gets the default valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        IConfigurationCollection GetConfiguration();

        /// <summary>
        /// Gets the input template for the given emulated controller port.
        /// </summary>
        /// <param name="emulatedDevice">The given device to create an input template for.</param>
        /// <returns>A valid input template for the given emulated device details.</returns>
        IInputTemplate GetInputTemplate(IEmulatedController emulatedDevice);
    }

    /// <summary>
    /// Generic version of <see cref="IConfigurationFactory"/>
    /// </summary>
    /// <typeparam name="TConfigurationCollection">The collection this factory produces.</typeparam>
    /// <typeparam name="TInputTemplate">The collection this factory produces.</typeparam>
    public interface IConfigurationFactory<out TConfigurationCollection, out TInputTemplate> : IConfigurationFactory
        where TConfigurationCollection : class, IConfigurationCollection<TConfigurationCollection>
        where TInputTemplate : class, IInputTemplate<TInputTemplate>
    {
        /// <summary>
        /// Gets the valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <seealso cref="IConfigurationCollectionStore"/>
        /// <param name="gameRecord">The game that is associated with this set of configuration collections</param>
        /// <param name="profileName">The profile name to get configuration for. By default, the profile name must be 'default'</param>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        new IConfigurationCollection<TConfigurationCollection> GetConfiguration(IGameRecord gameRecord, string profileName = "default");

        /// <summary>
        /// Gets the default valid set of configuration required to launch this emulator for a given game record.
        /// </summary>
        /// <returns>A set of configuration collection keyed on the expected file names of the configuration files.</returns>
        new IConfigurationCollection<TConfigurationCollection> GetConfiguration();

        /// <summary>
        /// Gets the input template for the given emulated controller port.
        /// </summary>
        /// <param name="emulatedDevice">The given device to create an input template for.</param>
        /// <returns>A valid input template for the given emulated device details.</returns>
        new IInputTemplate<TInputTemplate> GetInputTemplate(IEmulatedController emulatedDevice);
    }
}
