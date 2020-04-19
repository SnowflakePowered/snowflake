using HotChocolate;
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
        IGraphQLSchemaRegistrationProvider AddQuery<TQueryType, TQueryBuilder>(TQueryBuilder queryBuilderInstance) where TQueryType 
            : ObjectType<TQueryBuilder>;
        void ConfigureSchema(string schemaNamespace, string schemaName, Action<ISchemaBuilder> schemaBuilder);
    }
}
