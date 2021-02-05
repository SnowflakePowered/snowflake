using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Configuration;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Remoting.Kestrel;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Model.Game;
using HotChocolate.Language;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Queueing;

using Snowflake.Support.GraphQL.Server;

namespace Snowflake.Services
{
    internal class HotChocolateKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        public HotChocolateKestrelIntegration(HotChocolateSchemaBuilder schema)
        {
            this.Schema = schema;
        }

        private HotChocolateSchemaBuilder Schema { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Add privileged newtypes for Stone
            services
                .AddRouting()
                .AddMemoryCache()
                ;
            var graphQL = services
                .AddGraphQLServer()
                .AddApolloTracing()
                .AddInMemorySubscriptions()
                .UseAutomaticPersistedQueryPipeline()
                .AddInMemoryQueryStorage();

            this.Schema.AddSnowflakeGraphQl(graphQL);
            this.Schema.AddStoneIdTypeConverters(graphQL);
            this.Schema.AddSnowflakeQueryRequestInterceptor(graphQL);
           
        }
    }
}
