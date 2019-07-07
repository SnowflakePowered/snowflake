using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public abstract class AbstractStringConfigurationSerializer
        : ConfigurationSerializer<string>
    {
        public override string Transform(IAbstractConfigurationNode node)
        {
            IConfigurationSerializationContext<string> context = new StringSerializationContext();
            this.SerializeHeader(context);
            this.SerializeNode(node, context);
            this.SerializeFooter(context);
            return context.ToString();
        }
    }
}
