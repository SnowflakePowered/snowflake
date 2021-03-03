using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public abstract record AbstractConfigurationNode<T>
        : IAbstractConfigurationNode, IAbstractConfigurationNode<T>
    {
        private protected AbstractConfigurationNode(string key, T value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; }
        public T Value { get; }
        object? IAbstractConfigurationNode.Value => this.Value;
    }
}
