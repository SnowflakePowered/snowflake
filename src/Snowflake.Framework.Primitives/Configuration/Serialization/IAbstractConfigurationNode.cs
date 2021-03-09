using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Snowflake.Input.Controller;
namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Represents a configuration node with a key and a value.
    /// 
    /// Configuration nodes are an abstraction over key value pairs, where
    /// value could be a terminal node or a list of more nodes.
    /// </summary>
    public interface IAbstractConfigurationNode
    {
        /// <summary>
        /// The key, or name of the node. The key of a node must be representable as a
        /// <see cref="string"/>.
        /// 
        /// In general, nodes are transformed into some form Key = Value, whether in a
        /// serialzied format or a configuration object.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The value this node encapsulates.
        /// </summary>
        object? Value { get; }

        /// <summary>
        /// A list of annotations associated with this node.
        /// </summary>
        ImmutableArray<NodeAnnotation> Annotations { get; }
    }

    /// <summary>
    /// Represents a configuration node with a key and a value.
    /// 
    /// Configuration nodes are an abstraction over key value pairs, where
    /// value could be a terminal node or a list of more nodes.
    /// </summary>
    /// <typeparam name="T">One of 
    /// <see cref="string"/>, <see cref="bool"/>, <see cref="long"/>, <see cref="double"/>, <see cref="ControllerElement"/>, 
    /// <see cref="Enum"/>, <see cref="IReadOnlyList{TNode}"/>, where T is <see cref="IAbstractConfigurationNode"/>
    /// </typeparam>
    public interface IAbstractConfigurationNode<T>
        : IAbstractConfigurationNode
    {
        /// <summary>
        /// The value this node encapsulates.
        /// </summary>
        new T Value { get; }
    }
}
