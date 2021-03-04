using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Snowflake.Configuration.Serialization
{
    public sealed record ListConfigurationNode
        : AbstractConfigurationNode<ImmutableArray<IAbstractConfigurationNode>>
    {
        internal ListConfigurationNode(string key, ImmutableArray<IAbstractConfigurationNode> value) 
            : base(key, value)
        {
        }
    }
}
