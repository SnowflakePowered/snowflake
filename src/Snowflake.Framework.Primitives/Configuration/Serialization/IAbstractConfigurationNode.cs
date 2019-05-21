using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
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
