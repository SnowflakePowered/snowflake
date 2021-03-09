using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// A configuration node that represents a terminal <see cref="string"/> node.
    /// </summary>
    public sealed record StringConfigurationNode
        : AbstractConfigurationNode<string>
    {
        public StringConfigurationNode(string key, string value) : base(key, value)
        {
        }
    }
}
