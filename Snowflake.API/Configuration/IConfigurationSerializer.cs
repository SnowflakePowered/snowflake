﻿using System.Collections.Generic;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Emulator;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration serializer serializes a ConfigurationSection into valid emulator configuration.
    /// If an emulator uses a different syle of configuration, re-implement ConfigurationSerializer for that emulator instead of
    /// manually using string templates.
    /// <see cref="Snowflake.Configuration.IConfigurationSerializer"/>
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
        /// Serializes a controller element line using the provided input mapper.
        /// </summary>
        /// <param name="key">The key of the option</param>
        /// <param name="element">The controller element to serialize</param>
        /// <param name="inputMapping">The input mapping to serialize with</param>
        /// <returns></returns>
        string SerializeInput(string key, IMappedControllerElement element, IInputMapping inputMapping);

        /// <summary>
        /// Serializes the specified configuration section.
        /// </summary>
        /// <param name="configurationSection">The configuration section object to serialize</param>
        /// <returns>The entire section serialized as a string</returns>
        string Serialize(IConfigurationSection configurationSection);

        /// <summary>
        /// Serializes the specified input template.
        /// </summary>
        /// <param name="inputTemplate">The input template to serialize</param>
        /// <param name="inputMapping">The input mapping to serialize with</param>
        /// <returns>The entire input template serialized as a string</returns>
        string Serialize(IInputTemplate inputTemplate, IInputMapping inputMapping);
    }
}