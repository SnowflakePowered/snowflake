using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a terminal node of unknown type.
    /// Most serializers will ignore this, and it is here for developer convenience.
    /// </summary>
    public sealed record UnknownConfigurationNode
        : AbstractConfigurationNode<object>
    {
        internal UnknownConfigurationNode(string key, object value) : base(key, value)
        {
        }
    }
}
