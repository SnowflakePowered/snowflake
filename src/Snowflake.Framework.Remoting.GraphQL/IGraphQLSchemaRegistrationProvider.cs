using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Snowflake.Loader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL
{
    /// <summary>
    /// Provides access to the root GraphQL Schema.
    /// 
    /// To prevent conflicts, you should only ever manually add types that belong to the calling assembly.
    /// </summary>
    public interface IGraphQLSchemaRegistrationProvider
    {
        /// <summary>
        /// Adds an empty type to be used for extension to the Query type.
        /// </summary>
        /// <param name="fieldName">The field name to access this query.</param>
        /// <param name="typeName">The name of the extension type.</param>
        /// <param name="description">The description of the extension type.</param>
        IGraphQLSchemaRegistrationProvider AddEmptyQueryType(string fieldName, string typeName, string description);

        /// <summary>
        /// Adds an enumeration type to the GraphQL schema.
        /// </summary>
        /// <typeparam name="T">The enumeration type definition as an <see cref="EnumType"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddEnumType<T>() where T : EnumType;

        /// <summary>
        /// Adds an interface type to the GraphQL schema.
        /// </summary>
        /// <typeparam name="T">The interface type definition as an <see cref="InterfaceType"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddInterfaceType<T>() where T
           : InterfaceType;

        /// <summary>
        /// Adds an object type to the GraphQL schema.
        /// </summary>
        /// <typeparam name="T">The object type definition as an <see cref="ObjectType"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddObjectType<T>() where T
           : ObjectType;

        /// <summary>
        /// Extend an existing object type in the GraphQL schema.
        /// 
        /// This is mostly used to extend the root Query, Mutation, and Subscription types, but can be used to extend other types as well.
        /// See <seealso cref="SnowflakeGraphQLExtensions.ExtendQuery(IObjectTypeDescriptor)"/>, <seealso cref="SnowflakeGraphQLExtensions.ExtendMutation(IObjectTypeDescriptor)"/>, 
        /// <seealso cref="SnowflakeGraphQLExtensions.ExtendMutation(IObjectTypeDescriptor)"/>
        /// </summary>
        /// <typeparam name="T">The object type extension as a <see cref="ObjectTypeExtension"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddObjectTypeExtension<T>() where T
          : ObjectTypeExtension;

        /// <summary>
        /// Extends an existing interface type in the GraphQL schema.
        /// </summary>
        /// <typeparam name="T">The interface type extension as a <see cref="InterfaceTypeExtension"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddInterfaceTypeExtension<T>() where T
         : InterfaceTypeExtension;

        /// <summary>
        /// Adds a new scalar type to the GraphQL schema.
        /// </summary>
        /// <typeparam name="T">The scalar type as a <see cref="ScalarType"/> subtype.</typeparam>
        IGraphQLSchemaRegistrationProvider AddScalarType<T>() where T
           : ScalarType;

        /// <summary>
        /// Manually configure the <see cref="ISchemaBuilder"/> used to configure the schema.
        /// </summary>
        /// <param name="schemaBuilder">A delegate that configures the <see cref="ISchemaBuilder"/></param>
        IGraphQLSchemaRegistrationProvider ConfigureSchema(Action<ISchemaBuilder> schemaBuilder);

        /// <summary>
        /// Configure the <see cref="IQueryRequestBuilder"/> used to build the GraphQL query request
        /// when serving a query.
        /// </summary>
        /// <param name="queryBuilder">A delegate that configures the <see cref="IQueryRequestBuilder"/>.</param>
        IGraphQLSchemaRegistrationProvider ConfigureQueryRequest(Action<IQueryRequestBuilder> queryBuilder);
    }
}
