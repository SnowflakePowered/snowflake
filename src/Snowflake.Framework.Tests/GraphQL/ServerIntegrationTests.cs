using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Services;
using Snowflake.Support.GraphQL.Server;
using Snowflake.Support.GraphQLFrameworkQueries.Containers;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Remoting.GraphQL.Tests
{
    public class ServerIntegrationTests
    {
        [Fact]
        public async Task FrameworkSchemaCreation_Tests()
        {
            var appDataDirectory = new DirectoryInfo(System.IO.Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var container = new ServiceContainer(appDataDirectory.FullName, "http://localhost:9797");
            var schema = new GraphQLSchemaRegistrationProvider(container.Get<ILogProvider>().GetLogger("graphql"));
            var schemaBuilder = new HotChocolateSchemaBuilder(schema, container.GetServiceContainerAsServiceProvider());
            IRequestExecutorBuilder builder = new ServiceCollection().AddGraphQL();
            schemaBuilder.AddSnowflakeGraphQl(builder, true);
            schemaBuilder.AddStoneIdTypeConverters(builder);
            await builder.BuildSchemaAsync();
        }

        [Fact]
        public async Task FrameworkQueryCreation_Tests()
        {
            var appDataDirectory = new DirectoryInfo(System.IO.Path.GetTempPath())
               .CreateSubdirectory(Guid.NewGuid().ToString());
            var container = new ServiceContainer(appDataDirectory.FullName, "http://localhost:9797");
            var schema = new GraphQLSchemaRegistrationProvider(container.Get<ILogProvider>().GetLogger("graphql"));
            var schemaBuilder = new HotChocolateSchemaBuilder(schema, container.GetServiceContainerAsServiceProvider());
            IRequestExecutorBuilder builder = new ServiceCollection().AddGraphQL();
            schemaBuilder.AddSnowflakeGraphQl(builder);
            schemaBuilder.AddStoneIdTypeConverters(builder);
            schemaBuilder.AddSnowflakeQueryRequestInterceptor(builder);
            new QueryContainer().ConfigureHotChocolate(schema);
            await builder.BuildSchemaAsync();

        }
    }
}
