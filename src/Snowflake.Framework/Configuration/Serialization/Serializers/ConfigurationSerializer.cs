using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Serialization;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public abstract class ConfigurationSerializer<T> : IConfigurationSerializer<T>
    {
        public abstract T Serialize(IAbstractConfigurationNode node);
        public virtual void SerializeHeader(IConfigurationSerializationContext<T> context)
        {
            return;
        }

        public virtual void SerializeFooter(IConfigurationSerializationContext<T> context)
        {
            return;
        }

        public void SerializeNode(IAbstractConfigurationNode node, IConfigurationSerializationContext<T> context, int index = 0)
        {
            switch (node)
            {
                case ListConfigurationNode listNode:
                    this.SerializeNode(listNode, context, index);
                    break;
                case StringConfigurationNode stringNode:
                    this.SerializeNode(stringNode, context, index);
                    break;
                case BooleanConfigurationNode boolNode:
                    this.SerializeNode(boolNode, context, index);
                    break;
                case IntegralConfigurationNode intNode:
                    this.SerializeNode(intNode, context, index);
                    break;
                case DecimalConfigurationNode decNode:
                    this.SerializeNode(decNode, context, index);
                    break;
                case NullConfigurationNode decNode:
                    this.SerializeNode(decNode, context, index);
                    break;
                case EnumConfigurationNode enumNode:
                    this.SerializeNode(enumNode, context, index);
                    break;
                default:
                    break;
            }
        }
        public abstract void SerializeBlockBegin(IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeBlockEnd(IConfigurationSerializationContext<T> context, int index);
        protected void SerializeNode(ListConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            // Ignore pseudo-targets
            if (!node.Key.StartsWith("#"))
            {
                context.EnterScope(node.Key);
                this.SerializeBlockBegin(context, index);
            }

            int childIndex = 0;
            foreach (var childNode in node.Value)
            {
                this.SerializeNode(childNode, context, childIndex);
                childIndex++;
            }

            if (!node.Key.StartsWith("#"))
            {
                this.SerializeBlockEnd(context, index);
                context.ExitScope();
            }
        }
        protected void SerializeNode(StringConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }
        protected void SerializeNode(BooleanConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }
        protected void SerializeNode(IntegralConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }
        protected void SerializeNode(DecimalConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }
        protected void SerializeNode(EnumConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue((node as AbstractConfigurationNode<Enum>).Value, node.Value, node.Key, context, index);
        }
        protected void SerializeNode(NullConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Key, context, index);
        }

        public abstract void SerializeNodeValue(bool value, string key, IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeNodeValue(double value, string key, IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeNodeValue(Enum value, string enumValue, string key, IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeNodeValue(long value, string key, IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeNodeValue(string value, string key, IConfigurationSerializationContext<T> context, int index);
        public abstract void SerializeNodeValue(string key, IConfigurationSerializationContext<T> context, int index);
    }
}
