using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Remoting.GraphQL.Attributes
{
    /// <summary>
    /// Describes a parameter in a method's argument list, providing necesssary details about
    /// a given parameter so that it may be called from GraphQL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ParameterAttribute : Attribute
    {
        /// <summary>
        /// The native CLR type of the parameter.
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        /// The type of the GraphQL <see cref="ObjectGraphType"/> or <see cref="InputObjectGraphType"/> conversion of
        /// the <see cref="ParameterType"/>.
        /// </summary>
        public Type GraphQlType { get; }

        /// <summary>
        /// The name of the argument as it appears in the method's argument list.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// A description of the argument for use in GraphQL documentation introspection.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Whether or not the parameter is nullable, or strictly non-null.
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// Specifies a type to convert the <see cref="ParameterType"/> into before applying the <see cref="GraphQlType"/>
        /// transformation. Currently unimplemented.
        /// </summary>
        [Obsolete("Auto-conversion of parameter types is currently unimplemented, and does nothing.")]
        public Type ConvertableType { get; }

        /// <summary>
        /// Describes a parameter in a method's argument list, providing necesssary details about
        /// a given parameter so that it may be called from GraphQL.
        /// </summary>
        /// <param name="parameterType">The native CLR type of the parameter.</param>
        /// <param name="graphQlType">
        /// The type of the GraphQL <see cref="ObjectGraphType"/> or <see cref="InputObjectGraphType"/> conversion of
        /// the <see cref="ParameterType"/>.
        /// </param>
        /// <param name="parameterKey">The name of the argument as it appears in the method's argument list.</param>
        /// <param name="description">A description of the argument for use in GraphQL documentation introspection.</param>
        /// <param name="nullable">Whether or not the parameter is nullable, or strictly non-null.</param>
        /// <param name="convertableType">Reserved for future use.</param>
        public ParameterAttribute(Type parameterType, Type graphQlType, string parameterKey, string description,
            bool nullable = false, Type convertableType = null)
        {
            this.ParameterType = parameterType;
            this.Key = parameterKey;
            this.Description = description;
            this.Nullable = nullable;
            this.GraphQlType = nullable ? graphQlType : typeof(NonNullGraphType<>).MakeGenericType(graphQlType);
#pragma warning disable CS0618 // Type or member is obsolete
            this.ConvertableType = convertableType; // lgtm [cs/call-to-obsolete-method]
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
