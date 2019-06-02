using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Represents a configuration node with a key and a value
    /// </summary>
    public interface IAbstractConfigurationNode
    {
        string Key { get; }
        object? Value { get; }
    }

    public interface IAbstractConfigurationNode<T>
        : IAbstractConfigurationNode
    {
        new T Value { get; }
    }
}
