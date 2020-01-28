using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Framework.Remoting.Kestrel;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

namespace Snowflake.Services
{
    internal class GraphQLKestrelIntegration : IKestrelServerMiddlewareProvider
    {
        private GraphQLRootSchema GraphQLRootSchema { get; set; }

        public GraphQLKestrelIntegration(GraphQLRootSchema graphQLRootSchema)
        {
            this.GraphQLRootSchema = graphQLRootSchema;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseGraphQLWebSockets<GraphQLRootSchema>("/graphql");           
            app.UseGraphQL<GraphQLRootSchema>("/graphql");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.GraphQLRootSchema);
            
            // Add GraphQL services and configure options
            //services.AddGraphQL(options =>
            //{
            //    options.EnableMetrics = true;
            //    options.ExposeExceptions = true;
            //})
            //.AddWebSockets() // Add required services for web socket support
            //.AddDataLoader(); // Add required services for DataLoader support
        }
    }
}
