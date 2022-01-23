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
                .ModifyOptions(opts =>
                {
                    opts.DefaultBindingBehavior = BindingBehavior.Explicit;
                    opts.UseXmlDocumentation = true;
                    opts.StrictValidation = true;
                    opts.StrictRuntimeTypeValidation = false;
                    opts.RemoveUnreachableTypes = false;
                    opts.DefaultIsOfTypeCheck = (objectType, context, value) =>
                    {
                        // hack to ensure that DummyNodeType is never matched
                        // didn't need to do this because type was narrowed down before but since
                        // HC12 DummyNodeType has runtimetype Object.
                        // 
                        // In general, a runtime type of Object is poorly defined anyways.
                        if (objectType.RuntimeType == typeof(object)) {
                            return false;
                        }
                        return objectType.RuntimeType.IsInstanceOfType(value);
                    };
                })
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
