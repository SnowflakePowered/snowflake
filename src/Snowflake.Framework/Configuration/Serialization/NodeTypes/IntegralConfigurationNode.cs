using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class IntegralConfigurationNode
        : AbstractConfigurationNode<long>, IIntegralConfigurationNode
    {
        internal IntegralConfigurationNode(string key, long value) : base(key, value)
        {
        }
    }
}
