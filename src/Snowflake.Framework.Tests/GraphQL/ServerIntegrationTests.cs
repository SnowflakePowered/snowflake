//using HotChocolate;
//using Microsoft.Extensions.DependencyInjection;
//using Snowflake.Services;
//using Snowflake.Support.GraphQL.Server;
//using Snowflake.Support.GraphQLFrameworkQueries.Containers;
//using System;
//using System.IO;
//using Xunit;

//namespace Snowflake.Remoting.GraphQL.Tests
//{
//    public class ServerIntegrationTests
//    {
//        [Fact]
//        public void FrameworkSchemaCreation_Tests()
//        {
//            var appDataDirectory = new DirectoryInfo(System.IO.Path.GetTempPath())
//                .CreateSubdirectory(Guid.NewGuid().ToString());
//            var container = new ServiceContainer(appDataDirectory.FullName, "http://localhost:9797");
//            var schema = new GraphQLSchemaRegistrationProvider(container.Get<ILogProvider>().GetLogger("graphql"));
//            var schemaBuilder = new HotChocolateSchemaBuilder(schema, container.GetServiceContainerAsServiceProvider());
//            schemaBuilder.Create(true)
//                .Create()
//                .MakeExecutable();
//            //schemaBuilder.AddSnowflakeQueryRequestInterceptor(collection);
//            //schemaBuilder.AddStoneIdTypeConverters(collection);
//        }

//        [Fact]
//        public void FrameworkQueryCreation_Tests()
//        {
//            var appDataDirectory = new DirectoryInfo(System.IO.Path.GetTempPath())
//               .CreateSubdirectory(Guid.NewGuid().ToString());
//            var container = new ServiceContainer(appDataDirectory.FullName, "http://localhost:9797");
//            var schema = new GraphQLSchemaRegistrationProvider(container.Get<ILogProvider>().GetLogger("graphql"));
//            var schemaBuilder = new HotChocolateSchemaBuilder(schema, container.GetServiceContainerAsServiceProvider());
//            new QueryContainer().ConfigureHotChocolate(schema);
//            schemaBuilder.Create(false)
//                .Create()
//                .MakeExecutable();
//        }
//    }
//}
