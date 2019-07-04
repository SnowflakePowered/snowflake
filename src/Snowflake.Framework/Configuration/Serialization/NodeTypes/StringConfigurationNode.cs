using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class StringConfigurationNode
        : AbstractConfigurationNode<string>, IStringConfigurationNode
    {
        public StringConfigurationNode(string key, string value) : base(key, value)
        {
        }
    }
}
