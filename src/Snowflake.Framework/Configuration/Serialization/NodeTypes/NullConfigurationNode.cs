using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed class NullConfigurationNode
        : AbstractConfigurationNode<object?>
    {
        internal NullConfigurationNode(string key) : base(key, null)
        {
        }
    }
}
