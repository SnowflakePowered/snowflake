using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class NullConfigurationNode
        : AbstractConfigurationNode<object?>, INullConfigurationNode
    {
        internal NullConfigurationNode(string key) : base(key, null)
        {
        }
    }
}
