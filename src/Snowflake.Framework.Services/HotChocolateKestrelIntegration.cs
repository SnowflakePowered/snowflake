using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Stitching;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL.Model.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

namespace Snowflake.Services
{
    internal class HotChocolateKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        public HotChocolateKestrelIntegration(GraphQLSchemaRegistrationProvider schemas, IServiceProvider serviceCollection)
        {
            this.Schemas = schemas;
            this.ServiceCollection = serviceCollection;
        }

        public GraphQLSchemaRegistrationProvider Schemas { get; }
        public IServiceProvider ServiceCollection { get; }
        public IServiceProvider ServiceContainer { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseGraphQL("/hotchocolate");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataLoaderRegistry();
            services.AddGraphQLSubscriptions();
            
            var serviceProvider = services.BuildServiceProvider();
            
           
            services.AddStitchedSchema(builder =>
            {
                builder
                .AddSchemaConfiguration(c =>
                {
                    c.RegisterExtendedScalarTypes()
                        .RegisterType<PlatformIdType>()
                        .RegisterType<ControllerIdType>()
                        ;
                    c.RegisterServiceProvider(this.ServiceCollection);
                });
                

              
                foreach (var (schemaName, schemaBuilder) in this.Schemas.Schemas)
                {
                    schemaBuilder
                        .AddType<PlatformInfoType>()
                        .AddType<PlatformIdType>()
                        .AddType<ControllerIdType>()
                        ;

                    var schema = schemaBuilder.Create();

                    builder.AddSchema(schemaName, schema);
                }
            });
        }
    }
}
