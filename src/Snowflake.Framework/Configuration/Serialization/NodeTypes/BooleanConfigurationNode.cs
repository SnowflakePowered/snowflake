using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a terminal <see cref="bool"/> value.
    /// </summary>
    public sealed record BooleanConfigurationNode
        : AbstractConfigurationNode<bool>
    {
        internal BooleanConfigurationNode(string key, bool value) : base(key, value)
        {
        }
    }
}
