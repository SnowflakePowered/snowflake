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

        public void SerializeNode(IAbstractConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            switch (node)
            {
                case ListConfigurationNode listNode:
                    this.SerializeNode(listNode, context);
                    break;
                case StringConfigurationNode stringNode:
                    this.SerializeNode(stringNode, context);
                    break;
                case BooleanConfigurationNode boolNode:
                    this.SerializeNode(boolNode, context);
                    break;
                case IntegralConfigurationNode intNode:
                    this.SerializeNode(intNode, context);
                    break;
                case DecimalConfigurationNode decNode:
                    this.SerializeNode(decNode, context);
                    break;
                case NullConfigurationNode decNode:
                    this.SerializeNode(decNode, context);
                    break;
                case EnumConfigurationNode enumNode:
                    this.SerializeNode(enumNode, context);
                    break;
                default:
                    break;
            }
        }
        public abstract void SerializeBlockBegin(IConfigurationSerializationContext<T> context);
        public abstract void SerializeBlockEnd(IConfigurationSerializationContext<T> context);
        protected void SerializeNode(ListConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            context.EnterScope(node.Key);
            this.SerializeBlockBegin(context);
            foreach (var childNode in node.Value)
            {
                this.SerializeNode(childNode, context);
            }
            this.SerializeBlockEnd(context);
            context.ExitScope();
        }
        protected void SerializeNode(StringConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue(node.Value, node.Key, context);
        }
        protected void SerializeNode(BooleanConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue(node.Value, node.Key, context);
        }
        protected void SerializeNode(IntegralConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue(node.Value, node.Key, context);
        }
        protected void SerializeNode(DecimalConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue(node.Value, node.Key, context);
        }
        protected void SerializeNode(EnumConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue((node as AbstractConfigurationNode<Enum>).Value, node.Value, node.Key, context);
        }
        protected void SerializeNode(NullConfigurationNode node, IConfigurationSerializationContext<T> context)
        {
            this.SerializeNodeValue(node.Key, context);
        }
        public abstract void SerializeNodeValue(bool value, string key, IConfigurationSerializationContext<T> context);
        public abstract void SerializeNodeValue(double value, string key, IConfigurationSerializationContext<T> context);
        public abstract void SerializeNodeValue(Enum value, string enumValue, string key, IConfigurationSerializationContext<T> context);
        public abstract void SerializeNodeValue(long value, string key, IConfigurationSerializationContext<T> context);
        public abstract void SerializeNodeValue(string value, string key, IConfigurationSerializationContext<T> context);
        public abstract void SerializeNodeValue(string key, IConfigurationSerializationContext<T> context);
    }
}
