using System;
using System.Collections.Generic;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class ListConfigurationNode
        : AbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>, IListConfigurationNode
    {
        internal ListConfigurationNode(string key, IReadOnlyList<IAbstractConfigurationNode> value) 
            : base(key, value)
        {
        }
    }
}
