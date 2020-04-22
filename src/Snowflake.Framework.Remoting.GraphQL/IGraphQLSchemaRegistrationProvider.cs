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
    /// Only ever add a type that is in your own assembly
    /// </summary>
    public interface IGraphQLSchemaRegistrationProvider
    {
        IGraphQLSchemaRegistrationProvider AddEmptyQueryType(string fieldName, string typeName, string description);

        IGraphQLSchemaRegistrationProvider AddEnumType<T>() where T : EnumType;

        IGraphQLSchemaRegistrationProvider AddInterfaceType<T>() where T
           : InterfaceType;
        IGraphQLSchemaRegistrationProvider AddObjectType<T>() where T
           : ObjectType;
        IGraphQLSchemaRegistrationProvider AddObjectTypeExtension<T>() where T
          : ObjectTypeExtension;
        IGraphQLSchemaRegistrationProvider AddInterfaceTypeExtension<T>() where T
         : InterfaceTypeExtension;
        IGraphQLSchemaRegistrationProvider AddScalarType<T>() where T
           : ScalarType;
        IGraphQLSchemaRegistrationProvider ConfigureSchema(Action<ISchemaBuilder> schemaBuilder);
        IGraphQLSchemaRegistrationProvider ConfigureQueryRequest(Action<IQueryRequestBuilder> queryBuilder);
    }
}
