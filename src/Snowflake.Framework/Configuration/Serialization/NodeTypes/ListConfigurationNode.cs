using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a n-ary list of nested child nodes.
    /// </summary>
    public sealed record ListConfigurationNode
        : AbstractConfigurationNode<ImmutableArray<IAbstractConfigurationNode>>
    {
        internal ListConfigurationNode(string key, ImmutableArray<IAbstractConfigurationNode> value) 
            : base(key, value)
        {
        }
    }
}
