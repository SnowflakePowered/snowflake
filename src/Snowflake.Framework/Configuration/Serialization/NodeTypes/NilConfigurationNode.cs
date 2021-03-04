using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    public sealed record NilConfigurationNode
         : AbstractConfigurationNode<object?>
    {
        internal NilConfigurationNode() : base(String.Empty, null)
        {
        }
    }
}
