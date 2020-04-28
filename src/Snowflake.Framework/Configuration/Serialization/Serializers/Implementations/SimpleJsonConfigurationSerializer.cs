using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization.Serializers.Implementations
{
    public class SimpleJsonConfigurationSerializer
        : AbstractStringConfigurationSerializer
    {
        public SimpleJsonConfigurationSerializer()
        {
        }

        public override void SerializeHeader(IConfigurationSerializationContext<string> context)
        {
            context.Append("{");
        }

        public override void SerializeFooter(IConfigurationSerializationContext<string> context)
        {
            context.Append("}");
        }
        public override void SerializeBlockBegin( IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{context.GetCurrentScope()}"": {{");
        }
        public override void SerializeBlockEnd( IConfigurationSerializationContext<string> context, int index)
        {
            context.Append("}");
        }

        public override void SerializeNodeValue(bool value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":{value.ToString().ToLower()}");
        }

        public override void SerializeNodeValue(double value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":{value}");
        }

        public override void SerializeNodeValue(Enum enumValue, string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":""{value}""");
        }

        public override void SerializeNodeValue(long value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":{value}");
        }

        public override void SerializeNodeValue(string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":""{Sanitize(value)}""");
        }
 
        public override void SerializerNodeValue(ControllerElement controllerElementValue, string value, string key, IConfigurationSerializationContext<string> context, int index)
        {
            if (index != 0) context.Append(",");
            context.Append($@"""{key}"":""{value}""");
        }

        private static string Sanitize(string original)
        {
            return original.Replace("\\", "\\\\");
        }
    }
}
