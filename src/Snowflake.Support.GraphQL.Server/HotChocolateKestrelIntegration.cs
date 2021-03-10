using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Remoting.Kestrel;

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
                .AddMemoryCache();

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
