using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed class IntegralConfigurationNode
        : AbstractConfigurationNode<long>
    {
        internal IntegralConfigurationNode(string key, long value) : base(key, value)
        {
        }
    }
}
