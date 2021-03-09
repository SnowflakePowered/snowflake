using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Represents the lack of a configuration node. A fully parsed and visited tree should never contain any 
    /// <see cref="NilConfigurationNode"/>.
    /// </summary>
    public sealed record NilConfigurationNode
         : AbstractConfigurationNode<object?>
    {
        internal NilConfigurationNode() : base(String.Empty, null)
        {
        }
    }
}
