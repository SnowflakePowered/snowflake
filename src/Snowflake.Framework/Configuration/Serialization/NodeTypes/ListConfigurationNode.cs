using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed class ListConfigurationNode
        : AbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>
    {
        public ListConfigurationNode(string key, IReadOnlyList<IAbstractConfigurationNode> value) 
            : base(key, value)
        {
        }
    }
}
