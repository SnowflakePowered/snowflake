using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class BooleanConfigurationNode
        : AbstractConfigurationNode<bool>, IBooleanConfigurationNode
    {
        internal BooleanConfigurationNode(string key, bool value) : base(key, value)
        {
        }
    }
}
