using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class UnknownConfigurationNode
        : AbstractConfigurationNode<object>, IUnknownConfigurationNode
    {
        internal UnknownConfigurationNode(string key, object value) : base(key, value)
        {
        }
    }
}
