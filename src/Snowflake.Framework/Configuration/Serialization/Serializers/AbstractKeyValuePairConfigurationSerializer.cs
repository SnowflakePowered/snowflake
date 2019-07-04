using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Serialization;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public abstract class AbstractKeyValuePairConfigurationSerializer
        : AbstractStringConfigurationSerializer
    {
        protected abstract string Separator { get; }

        public override void SerializeNode(IStringConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}{node.Value}");
        }
        public override void SerializeNode(IBooleanConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}{node.Value}");
        }
        public override void SerializeNode(IIntegralConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}{node.Value}");
        }
        public override void SerializeNode(IDecimalConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}{node.Value}");
        }
        public override void SerializeNode(INullConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}");
        }
        public override void SerializeNode(IEnumConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{node.Key}{Separator}{node.Value}");
        }
    }
}
