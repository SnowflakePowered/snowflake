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
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL.Model.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

namespace Snowflake.Services
{
    internal class HotChocolateKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        public HotChocolateKestrelIntegration(GraphQLSchemaRegistrationProvider schemas)
        {
            this.Schemas = schemas;
        }

        public GraphQLSchemaRegistrationProvider Schemas { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseGraphQL("/hotchocolate");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add each query service
            foreach (var sd in this.Schemas.QueryBuilderServices)
            {
                services.Add(sd);
            }

            this.Schemas
                .AddScalarType<PlatformIdType>()
                .AddScalarType<ControllerIdType>();

            this.Schemas
                .AddObjectType<PlatformInfoType>();
;
            services.AddDataLoaderRegistry();
            services.AddGraphQLSubscriptions();
        
            services.AddStitchedSchema(builder =>
            {
                builder
                .AddSchemaConfiguration(c =>
                {
                    c.RegisterExtendedScalarTypes();
                    foreach (var type in this.Schemas.ScalarTypes)
                    {
                        c.RegisterType(type);
                    }
                });

                foreach (var (schemaName, schemaBuilder) in this.Schemas.Schemas)
                {
                    foreach (var type in this.Schemas.ScalarTypes)
                    {
                        schemaBuilder.AddType(type);
                    }

                    foreach (var type in this.Schemas.ObjectTypes)
                    {
                        schemaBuilder.AddType(type);
                    }

                    var schema = schemaBuilder.Create();

                    builder.AddSchema(schemaName, schema);
                }
            });
        }
    }
}
