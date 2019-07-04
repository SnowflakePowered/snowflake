using System;
using System.Collections.Generic;

namespace Snowflake.Configuration.Serialization
{
    public sealed class ListConfigurationNode
        : AbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>
    {
        internal ListConfigurationNode(string key, IReadOnlyList<IAbstractConfigurationNode> value) 
            : base(key, value)
        {
        }
    }
}
