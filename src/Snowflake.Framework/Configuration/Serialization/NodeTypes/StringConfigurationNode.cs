using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed record StringConfigurationNode
        : AbstractConfigurationNode<string>
    {
        public StringConfigurationNode(string key, string value) : base(key, value)
        {
        }
    }
}
