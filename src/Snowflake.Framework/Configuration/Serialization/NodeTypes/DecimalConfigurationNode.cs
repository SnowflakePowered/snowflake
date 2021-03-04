using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a terminal <see cref="decimal"/> value.
    /// </summary>
    public sealed record DecimalConfigurationNode
        : AbstractConfigurationNode<double>
    {
        internal DecimalConfigurationNode(string key, double value) : base(key, value)
        {
        }
    }
}
