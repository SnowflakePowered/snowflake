using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed record BooleanConfigurationNode
        : AbstractConfigurationNode<bool>
    {
        internal BooleanConfigurationNode(string key, bool value) : base(key, value)
        {
        }
    }
}
