using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed record DecimalConfigurationNode
        : AbstractConfigurationNode<double>
    {
        internal DecimalConfigurationNode(string key, double value) : base(key, value)
        {
        }
    }
}
