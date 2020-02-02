using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Attributes
{
    /// <summary>
    /// <para>
    /// A Connection is a common idiom in GraphQL that implements cursor-based pagination for enumerable objects.
    /// Methods marked with <see cref="ConnectionAttribute"/> must return <see cref="IEnumerable{T}"/>.
    /// if the returned <see cref="IEnumerable{T}"/> is impure, cursors may become
    /// potentialy meaningless, since nothing is cached between calls of the same method.
    /// </para>
    /// <para>
    /// A method can only be marked with one of <see cref="ConnectionAttribute"/>, <see cref="QueryAttribute"/>, or <see cref="MutationAttribute"/>,
    /// never more than one.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ConnectionAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="ObjectGraphType"/> conversion of the type that is contained with the enumerable return type.
        /// </summary>
        public Type GraphType { get; }
        /// <summary>
        /// The name of the GraphQL field to expose.
        /// </summary>
        public string FieldName { get; }
        /// <summary>
        /// The description of the field.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Marks a method as a GraphQL Connection. Connection methods must return <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="fieldName">The name of the GraphQL field to expose.</param>
        /// <param name="description">The description of the field.</param>
        /// <param name="graphType">The <see cref="ObjectGraphType"/> conversion of the type that is contained with the enumerable return type.</param>
        public ConnectionAttribute(string fieldName, string description, Type graphType)
        {
            this.GraphType = graphType;
            this.FieldName = fieldName;
            this.Description = description;

            if (!this.GraphType.GetInterfaces().Contains(typeof(IGraphType)))
            {
                throw new InvalidOperationException($"The connection type for {this.FieldName} must be a graph type.");
            }
        }
    }
}
