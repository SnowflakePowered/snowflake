using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a terminal <see cref="long"/> integral value.
    /// </summary>
    public sealed record IntegralConfigurationNode
        : AbstractConfigurationNode<long>
    {
        internal IntegralConfigurationNode(string key, long value) : base(key, value)
        {
        }
    }
}
