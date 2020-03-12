using HotChocolate;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Loader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Snowflake
{
    internal class GraphQLSchemaRegistrationProvider : IGraphQLSchemaRegistrationProvider
    {
        public GraphQLSchemaRegistrationProvider()
        {
            this.Schemas = new ConcurrentDictionary<string, ISchemaBuilder>();
        }

        internal IDictionary<string, ISchemaBuilder> Schemas { get; }

        public void RegisterSchema(
            IServiceRepository loaderServices, string schemaNamespace, string schemaName, Action<ISchemaBuilder> schemaBuilder)
        {
            var schema = SchemaBuilder.New()
                .AddServices(loaderServices.ToServiceProvider()); // Add loader services as SP

            schemaBuilder(schema);

            this.Schemas.Add($"{schemaNamespace}_{schemaName}", schema);
        }
    }
}
