using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization.Serializers.Implementations
{
    public class SimpleCfgConfigurationSerializer
        : AbstractStringConfigurationSerializer
    {
        public SimpleCfgConfigurationSerializer()
        {
        }

        public override void SerializeBlockBegin(IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"# {String.Join('.', context.GetFullScope())}");
        }
        public override void SerializeBlockEnd(IConfigurationSerializationContext<string> context, int index)
        {
            return;
        }

        public override void SerializeNodeValue(bool value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }

        public override void SerializeNodeValue(double value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }

        public override void SerializeNodeValue(Enum enumValue, string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }

        public override void SerializeNodeValue(long value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }

        public override void SerializeNodeValue(string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }

        public override void SerializerNodeValue(ControllerElement controllerElementValue, string value, string key, IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($@"{key}=""{value}""");
        }
    }
}
