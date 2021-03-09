using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public string Key { get; init; }

        public T Value { get; init; }

        public ImmutableArray<NodeAnnotation> Annotations { get; init; }

        object? IAbstractConfigurationNode.Value => this.Value;
    }
}
