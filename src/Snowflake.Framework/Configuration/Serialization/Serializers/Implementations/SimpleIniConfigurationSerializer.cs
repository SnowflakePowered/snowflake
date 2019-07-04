using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization.Serializers.Implementations
{
    public class SimpleIniSerializer
        : AbstractKeyValuePairConfigurationSerializer
    {
        public SimpleIniSerializer()
        {
        }
        protected override string Separator => "=";

        public override void SerializeBlockBegin(IListConfigurationNode block, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"[{String.Join('.', context.GetFullScope())}]");
        }
        public override void SerializeBlockEnd(IListConfigurationNode block, IConfigurationSerializationContext<string> context)
        {
            return;
        }
    }
}
