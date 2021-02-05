using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Extensibility;
using Snowflake.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Model.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using HotChocolate.Language;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;

namespace Snowflake.Services
{
    internal class GraphQLSchemaRegistrationProvider : IGraphQLSchemaRegistrationProvider
    {
        private static readonly object Empty = new { };
        public GraphQLSchemaRegistrationProvider(ILogger logger)
        {
            this.SchemaConfig = new List<Action<IRequestExecutorBuilder>>();
            this.QueryConfig = new List<Action<IQueryRequestBuilder>>();

            this.ObjectTypes = new List<Type>();
            this.ObjectTypeExtensions = new List<Type>();
            this.InterfaceTypes = new List<Type>();
            this.EnumTypes = new List<Type>();
            this.ScalarTypes = new List<Type>();
            this.Logger = logger;
        }

        internal IList<Type> ObjectTypes { get; }
        internal IList<Type> EnumTypes { get; }
        internal IList<Type> ObjectTypeExtensions { get; }
        internal IList<Type> ScalarTypes { get; }
        internal IList<Type> InterfaceTypes { get; }

        private ILogger Logger { get; }

        internal IList<Action<IRequestExecutorBuilder>> SchemaConfig { get; }
        internal IList<Action<IQueryRequestBuilder>> QueryConfig { get; }

        public IGraphQLSchemaRegistrationProvider AddObjectType<T>()
            where T : ObjectType
        {
            this.Logger.Info($"Registered GraphQL Object Type from {typeof(T).Name}.");
            this.ObjectTypes.Add(typeof(T));
            return this;
        }

        public IGraphQLSchemaRegistrationProvider AddScalarType<T>() where T : ScalarType
        {
            this.Logger.Info($"Registered GraphQL Scalar from {typeof(T).Name}.");
            this.ScalarTypes.Add(typeof(T));
            return this;
        }
        public IGraphQLSchemaRegistrationProvider AddEnumType<T>() where T : EnumType
        {
            this.Logger.Info($"Registered GraphQL Enum from {typeof(T).Name}.");
            this.EnumTypes.Add(typeof(T));
            return this;
        }

        public IGraphQLSchemaRegistrationProvider AddInterfaceType<T>() where T : InterfaceType
        {
            this.Logger.Info($"Registered GraphQL Interface from {typeof(T).Name}.");
            this.InterfaceTypes.Add(typeof(T));
            return this;
        }

        public IGraphQLSchemaRegistrationProvider AddObjectTypeExtension<T>() where T : ObjectTypeExtension
        {
            this.Logger.Info($"Registered GraphQL Object Type Extensions from {typeof(T).Name}.");
            this.ObjectTypeExtensions.Add(typeof(T));
            return this;
        }

        public IGraphQLSchemaRegistrationProvider AddInterfaceTypeExtension<T>() where T : InterfaceTypeExtension
        {
            this.Logger.Info($"Registered GraphQL Interface Type Extensions from {typeof(T).Name}.");
            this.ObjectTypeExtensions.Add(typeof(T));
            return this;
        }

        public IGraphQLSchemaRegistrationProvider ConfigureSchema(Action<IRequestExecutorBuilder> schemaBuilder)
        {
            this.SchemaConfig.Add(schemaBuilder);
            return this;
        }

        public IGraphQLSchemaRegistrationProvider ConfigureQueryRequest(Action<IQueryRequestBuilder> queryBuilder)
        {
            this.QueryConfig.Add(queryBuilder);
            return this;
        }

        public IGraphQLSchemaRegistrationProvider AddEmptyQueryType(string fieldName, string typeName, string description)
        {
            this.Logger.Info($"Registered GraphQL Query Type Extensions {typeName} at Query.{fieldName}.");
            this.SchemaConfig.Add(schemaBuilder =>
            {
                schemaBuilder.AddObjectType(descriptor =>
                {
                    descriptor.Name(typeName)
                        .Description(description);
                })
                .AddTypeExtension(new ObjectTypeExtension(descriptor =>
                {
                    descriptor.ExtendQuery();
                    descriptor.Field(fieldName)
                    .Description(description)
                    .Type(new NamedTypeNode(typeName))
                    .Resolve(GraphQLSchemaRegistrationProvider.Empty);
                }));
            });
            return this;
        }
    }
}
