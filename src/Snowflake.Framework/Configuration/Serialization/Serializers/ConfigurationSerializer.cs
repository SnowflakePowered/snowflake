using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization.Serializers
{
    /// <summary>
    /// Implements <see cref="IConfigurationTransformer{TOutput}"/> by serializing the 
    /// syntax tree into some string or binary format.
    /// </summary>
    /// <typeparam name="T">The type of the serialized data after traversing the tree.</typeparam>
    public abstract class ConfigurationSerializer<T> : IConfigurationTransformer<T>
    {
        /// <inheritdoc />
        public abstract T Transform(IAbstractConfigurationNode node);

        /// <summary>
        /// Write the header to the serialized stream.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        public virtual void SerializeHeader(IConfigurationSerializationContext<T> context)
        {
            return;
        }

        /// <summary>
        /// Write the header to the serialized stream.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        public virtual void SerializeFooter(IConfigurationSerializationContext<T> context)
        {
            return;
        }

        /// <summary>
        /// Serialize a <see cref="IAbstractConfigurationNode"/> with the given context.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public void SerializeNode(IAbstractConfigurationNode node,
            IConfigurationSerializationContext<T> context, int index = 0)
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
                case ControllerElementConfigurationNode ctrlNode:
                    this.SerializeNode(ctrlNode, context, index);
                    break;
                case EnumConfigurationNode enumNode:
                    this.SerializeNode(enumNode, context, index);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Serialize a marker or header where a block or section begins in the serialized stream.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeBlockBegin(IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serialize a marker or footer where a block or section ends in the serialized stream.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeBlockEnd(IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes a list of configuration nodes.
        /// 
        /// For each child node to be serialized, the index should be incremented. Serializing a list node will 
        /// enter a new block, and thus block headers and footers are written at the beginning and end of
        /// serialization of this node respectively.
        /// </summary>
        /// <param name="node">The list of configuration nodes represented as a <see cref="ListConfigurationNode"/></param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
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

        /// <summary>
        /// Serializes a configuration node that encapsulates a <see cref="string"/>.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(StringConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes a configuration node that encapsulates a <see cref="bool"/>.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(BooleanConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes a configuration node that encapsulates an integral value, implemented as a <see cref="long"/>
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(IntegralConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes a configuration node that encapsulates a decimal value, implemented as a <see cref="double"/>.
        /// 
        /// Contrary to the name, this is not implemented as a <see cref="decimal"/>, and IEEE 754 floating point semantics
        /// for double precision floating point numbers should be taken into account.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(DecimalConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue(node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes a configuration node that encapsulates an <see cref="Enum"/> value.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(EnumConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue((node as AbstractConfigurationNode<Enum>).Value, node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes a configuration node that encapsulates a <see cref="ControllerElement"/>.
        /// 
        /// This is only used when serializing syntax trees that came from <see cref="IInputTemplate"/>.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        protected void SerializeNode(ControllerElementConfigurationNode node, IConfigurationSerializationContext<T> context, int index)
        {
            this.SerializeNodeValue((node as AbstractConfigurationNode<ControllerElement>).Value, node.Value, node.Key, context, index);
        }

        /// <summary>
        /// Serializes the value of a <see cref="BooleanConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="value">The raw value of the node.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeNodeValue(bool value, string key, IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes the value of a <see cref="DecimalConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="value">The raw value of the node.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeNodeValue(double value, string key, IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes the value of a <see cref="EnumConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="enumValue">The value of the node as the <see cref="Enum"/> object.</param>
        /// <param name="value">The raw value of the node.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeNodeValue(Enum enumValue, string value, string key, IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes the value of a <see cref="IntegralConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="value">The raw value of the node.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeNodeValue(long value, string key, IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes the value of a <see cref="StringConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="value">The raw value of the node.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializeNodeValue(string value, string key, IConfigurationSerializationContext<T> context, int index);

        /// <summary>
        /// Serializes the value of a <see cref="EnumConfigurationNode"/>. Override this when implementing a serializer.
        /// </summary>
        /// <param name="controllerElementValue">The value of the node</param>
        /// <param name="value">
        /// The raw value of the node as the string representation of the element consistent with the 
        /// <see cref="IInputMapping"/> the syntax tree was originally serialized with.</param>
        /// <param name="key">The key of the node.</param>
        /// <param name="context">The serialization context.</param>
        /// <param name="index">The position or index of the given node within the current block in the context.</param>
        public abstract void SerializerNodeValue(ControllerElement controllerElementValue, string value, string key, IConfigurationSerializationContext<T> context, int index);
    }
}
