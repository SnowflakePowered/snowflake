using Snowflake.Configuration.Serialization;
using System;
namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Transforms a tree of <see cref="IAbstractConfigurationNode"/> into a concrete representation. 
    /// 
    /// Although also used for serialization, <typeparamref name="TOutput"/> does not need to be 
    /// an opaque serialized type such as <see cref="System.String"/> or a <see cref="System.Byte"/>[],
    /// but could also be a configuration object that is less unwieldy to use than a full
    /// <see cref="IConfigurationCollection{T}"/>.
    /// 
    /// </summary>
    /// <typeparam name="TOutput">The concrete representation resulting from a traversal of the
    /// syntax tree the provided <see cref="IAbstractConfigurationNode"/> represents.
    /// </typeparam>
    public interface IConfigurationTransformer<TOutput>
    {
        /// <summary>
        /// Transforms a tree of <see cref="IAbstractConfigurationNode"/> into a concrete representation. 
        /// 
        /// See implementation assemblies for the set of node types that should be handled
        /// through recursive descent.
        /// </summary>
        /// <param name="node">The root node of the syntax tree.</param>
        /// <returns>The transformed output of the syntax tree.</returns>
        TOutput Transform(IAbstractConfigurationNode node);
    }
}