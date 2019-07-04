using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Serialization;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public abstract class AbstractStringConfigurationSerializer : IConfigurationSerializer<string>
    {
        public string SerializeNode(IAbstractConfigurationNode node)
        {
            IConfigurationSerializationContext<string> context = new StringSerializationContext();
            this.SerializeNode(node, context);
            return context.ToString();
        }

        public void SerializeNode(IAbstractConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            switch (node)
            {
                case IListConfigurationNode listNode:
                    this.SerializeNode(listNode, context);
                    break;
                case IStringConfigurationNode stringNode:
                    this.SerializeNode(stringNode, context);
                    break;
                case IBooleanConfigurationNode boolNode:
                    this.SerializeNode(boolNode, context);
                    break;
                case IIntegralConfigurationNode intNode:
                    this.SerializeNode(intNode, context);
                    break;
                case IDecimalConfigurationNode decNode:
                    this.SerializeNode(decNode, context);
                    break;
                case INullConfigurationNode decNode:
                    this.SerializeNode(decNode, context);
                    break;
                case IEnumConfigurationNode enumNode:
                    this.SerializeNode(enumNode, context);
                    break;
                default:
                    break;
            }
        }
        public abstract void SerializeBlockBegin(IListConfigurationNode block, IConfigurationSerializationContext<string> context);
        public abstract void SerializeBlockEnd(IListConfigurationNode block, IConfigurationSerializationContext<string> context);

        public void SerializeNode(IListConfigurationNode node, IConfigurationSerializationContext<string> context)
        {
            context.EnterScope(node.Key);
            this.SerializeBlockBegin(node, context);
            foreach (var childNode in node.Value)
            {
                this.SerializeNode(childNode, context);
            }
            this.SerializeBlockEnd(node, context);
            context.ExitScope();
        }

        public abstract void SerializeNode(IStringConfigurationNode node, IConfigurationSerializationContext<string> context);
        public abstract void SerializeNode(IBooleanConfigurationNode node, IConfigurationSerializationContext<string> context);
        public abstract void SerializeNode(IIntegralConfigurationNode node, IConfigurationSerializationContext<string> context);
        public abstract void SerializeNode(IDecimalConfigurationNode node, IConfigurationSerializationContext<string> context);
        public abstract void SerializeNode(INullConfigurationNode node, IConfigurationSerializationContext<string> context);
        public abstract void SerializeNode(IEnumConfigurationNode node, IConfigurationSerializationContext<string> context);

    }
}
