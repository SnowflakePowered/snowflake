using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class DecimalConfigurationNode
        : AbstractConfigurationNode<double>, IDecimalConfigurationNode
    {
        internal DecimalConfigurationNode(string key, double value) : base(key, value)
        {
        }
    }
}
