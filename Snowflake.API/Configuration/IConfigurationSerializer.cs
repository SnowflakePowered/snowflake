using System.Collections.Generic;
using Snowflake.Emulator;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration serializer serializes a ConfigurationSection into valid emulator configuration.
    /// If an emulator uses a different syle of configuration, re-implement ConfigurationSerializer for that emulator instead of
    /// manually using string templates.
    /// <see cref="Snowflake.Configuration.ConfigurationSerializer"/>
    /// </summary>
    public interface IConfigurationSerializer
    {
        /// <value>
        /// The type mapper for this serializer.
        /// <see cref="Snowflake.Configuration.IConfigurationTypeMapper"/>
        /// </value>
        IConfigurationTypeMapper TypeMapper { get; set; }
        
        /// <summary>
        /// Serializes one single value using the type mapper. Usually not very useful as this does not serialize a line and should 
        /// simply shim the TypeMapper, using reflection to get the runtime type of the objeect. Intended as a helper method for
        /// <see cref="SerializeLine{T}(string, T)"></see>
        /// <seealso cref="SerializeIterableLine{T}(string, T, int)"/>
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <returns>The value serialized with the typemapper</returns>
        string SerializeValue(object value);
        
        /// <summary>
        /// Serializes a line in the configuration with a key and a value
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize. Ensure the TypeMapper is setup to handle this type</typeparam>
        /// <param name="key">The key of the option</param>
        /// <param name="value">The value to serialize</param>
        /// <returns>The serialized line as it would appear in the emulator configuration</returns>
        string SerializeLine<T>(string key, T value);

        /// <summary>
        /// Serializes a line with an iteration value. 
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize. Ensure the TypeMapper is setup to handle this type</typeparam>
        /// <param name="key">The key of the option. By convention, any key names with '{N}' in it's OptionName will be replaced with the iteration number</param>
        /// <param name="value">The value to serialize</param>
        /// <param name="iteration">The iteration number to serialize '{N}' with</param>
        /// <returns>The serialized line as it would appear in the emulator configuration</returns>
        string SerializeIterableLine<T>(string key, T value, int iteration);

        /// <summary>
        /// Serializes the specified configuration section, in an instance-unaware way.
        /// </summary>
        /// <param name="configurationSection">The configuration section object to serialize</param>
        /// <returns>The entire section serialized as a string</returns>
        string Serialize(IConfigurationSection configurationSection);

        /// <summary>
        /// Serializes the specified iterable configuration section. 
        /// This method should call SerializeIterableLine on the IterationNumber of the given section.
        /// </summary>
        /// <param name="iterableConfigurationSection">The configuration section object to serialize</param>
        /// <returns>The entire section serialized as a string</returns>
        string Serialize(IIterableConfigurationSection iterableConfigurationSection);
    }
}