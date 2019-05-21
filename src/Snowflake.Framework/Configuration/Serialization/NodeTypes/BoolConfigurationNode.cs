using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed class BoolConfigurationNode
        : AbstractConfigurationNode<bool>
    {
        public BoolConfigurationNode(string key, bool value) : base(key, value)
        {
        }
    }
}
