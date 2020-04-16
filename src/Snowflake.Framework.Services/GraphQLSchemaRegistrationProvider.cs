using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Model.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Snowflake.Services
{
    internal class GraphQLSchemaRegistrationProvider : IGraphQLSchemaRegistrationProvider
    {
        public GraphQLSchemaRegistrationProvider(ILogger logger)
        {
            this.QueryBuilderServices = new ServiceCollection();
            this.Schemas = new ConcurrentDictionary<string, ISchemaBuilder>();
            this.ObjectTypes = new List<Type>();
            this.ObjectTypeExtensions = new List<Type>();
            this.InterfaceTypes = new List<Type>();
            this.EnumTypes = new List<Type>();
            this.ScalarTypes = new List<Type>();
            this.Logger = logger;
        }

        internal IServiceCollection QueryBuilderServices { get; }
        internal IList<Type> ObjectTypes { get; }
        internal IList<Type> EnumTypes { get; }
        internal IList<Type> ObjectTypeExtensions { get; }
        internal IList<Type> ScalarTypes { get; }
        internal IList<Type> InterfaceTypes { get; }

        private ILogger Logger { get; }

        internal IDictionary<string, ISchemaBuilder> Schemas { get; }

        public IGraphQLSchemaRegistrationProvider AddQuery<TQueryType, TQueryBuilder>(TQueryBuilder queryBuilderInstance)
            where TQueryType : ObjectType<TQueryBuilder>
        {
            string schemaName = typeof(TQueryBuilder).FullName;
            this.Logger.Info($"Registered GraphQL Queries from {schemaName}.");
            this.QueryBuilderServices.AddSingleton(typeof(TQueryBuilder), queryBuilderInstance);
            this.Schemas.Add(schemaName.Replace('.', '_'), SchemaBuilder.New()
                .AddQueryType<TQueryType>());
            return this;
        }

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

        public void ConfigureSchema(string schemaNamespace, string schemaName, Action<ISchemaBuilder> schemaBuilder)
        {
            var schemaBuilderInstance = SchemaBuilder.New();
            schemaBuilder(schemaBuilderInstance);
            this.Schemas.Add($"{schemaNamespace}_{schemaName}", schemaBuilderInstance);
            this.Logger.Info($"Registered GraphQL Schema {schemaNamespace}_{schemaName}");
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
    }
}
