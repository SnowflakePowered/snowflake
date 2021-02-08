using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization.Serializers.Implementations
{
    public class SimpleXmlConfigurationSerializer
        : AbstractStringConfigurationSerializer
    {
        public SimpleXmlConfigurationSerializer(string rootElementName)
        {
            this.RootElementName = rootElementName;
        }

        private string RootElementName { get; }

        public override void SerializeHeader(IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"<{this.RootElementName}>");
        }

        public override void SerializeFooter(IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"</{this.RootElementName}>");
        }

        public override void SerializeBlockBegin(IConfigurationSerializationContext<string> context, int index)
        {
            // todo: sanitize
            context.AppendLine($"<{context.GetCurrentScope()}>");
        }
        public override void SerializeBlockEnd(IConfigurationSerializationContext<string> context, int index)
        {
            // todo: sanitize
            context.AppendLine($"</{context.GetCurrentScope()}>");
        }
        public override void SerializeNodeValue(bool value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }
        public override void SerializeNodeValue(double value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }
        public override void SerializeNodeValue(Enum enumValue, string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }
        public override void SerializeNodeValue(long value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }
        public override void SerializeNodeValue(string value, string key,  IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }

        public override void SerializerNodeValue(ControllerElement controllerElementValue, string value, string key, IConfigurationSerializationContext<string> context, int index)
        {
            context.AppendLine($"<{key}>{value}</{key}>");
        }
    }
}
