using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Model.Game;
using Snowflake.Remoting.Kestrel;
using HotChocolate.Data.Filters;
using Snowflake.Support.GraphQL.Server;
using Snowflake.Input.Controller;
using HotChocolate.Data;
using HotChocolate.Types.Pagination;
using HotChocolate.Types;

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
                .UseWebSockets()
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
                .AddMemoryCache();

            var graphQL = services
                .AddGraphQLServer()
                
                .AddGlobalObjectIdentification()
                .AddInMemorySubscriptions()
                .UseAutomaticPersistedQueryPipeline()
                .AddInMemoryQueryStorage()
                .AddApolloTracing(HotChocolate.Execution.Options.TracingPreference.OnDemand);
            
            this.Schema.AddSnowflakeGraphQl(graphQL);
            this.Schema.AddStoneIdTypeConverters(graphQL);
            this.Schema.AddSnowflakeQueryRequestInterceptor(graphQL);
        }
    }
}
