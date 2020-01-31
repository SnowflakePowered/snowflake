using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Attributes
{
    /// <summary>
    /// <para>
    /// A mutation field in this context is simply a normal field on the mutation root object.
    /// By convention, only fields in the mutation root object may mutate state, as compared to <see cref="FieldAttribute"/>,
    /// which will attach the method as a field on the query root object instead.
    /// </para>
    /// <para>
    /// A method can only be marked with one of <see cref="ConnectionAttribute"/>, <see cref="FieldAttribute"/>, or <see cref="MutationAttribute"/>,
    /// never more than one.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MutationAttribute : Attribute
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
        /// Marks a method as a GraphQL field on the mutation root object./>.
        /// </summary>
        /// <param name="fieldName">The name of the GraphQL field to expose.</param>
        /// <param name="description">The description of the field.</param>
        /// <param name="graphType">The <see cref="ObjectGraphType"/> conversion of the return type.</param>
        public MutationAttribute(string fieldName, string description, Type graphType)
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
