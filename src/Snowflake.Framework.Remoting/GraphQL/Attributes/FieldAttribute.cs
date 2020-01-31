using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Attributes
{
    /// <summary>
    /// <para>
    /// A GraphQL field in this context is specifically a field on the query root object. This attribute will mark
    /// a method as being a GraphQL field on the query root object, and thus makes a method available to the GraphQL query schema.
    /// </para>
    /// <para>
    /// A method can only be marked with one of <see cref="ConnectionAttribute"/>, <see cref="FieldAttribute"/>, or <see cref="MutationAttribute"/>,
    /// never more than one.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class FieldAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="ObjectGraphType"/> conversion of the return type.
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
        /// Marks a method as a basic GraphQL field./>.
        /// </summary>
        /// <param name="fieldName">The name of the GraphQL field to expose.</param>
        /// <param name="description">The description of the field.</param>
        /// <param name="graphType">The <see cref="ObjectGraphType"/> conversion of the return type.</param>
        public FieldAttribute(string fieldName, string description, Type graphType)
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
