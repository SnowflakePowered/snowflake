using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Framework.Remoting.Kestrel;

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
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            })
            .AddWebSockets() // Add required services for web socket support
            .AddDataLoader(); // Add required services for DataLoader support
        }
    }

    internal class GraphQLRootSchema : Schema, IGraphQLService
    {
        public GraphQLRootSchema(IServiceContainer services)
        {
            this.Query = new RootQuery();
            this.Mutation = new RootMutation();
            services.Get<IKestrelWebServerService>()
                .AddService(new GraphQLKestrelIntegration(this));
        }

        /// <inheritdoc/>
        public void Register(QueryBuilder queries)
        {
            queries.RegisterConnectionQueries((RootQuery)this.Query);
            queries.RegisterFieldQueries((RootQuery)this.Query);
            queries.RegisterMutationQueries((RootMutation)this.Mutation);
        }
    }

    internal class RootQuery : ObjectGraphType<object>
    {
        public RootQuery()
        {
            this.Name = "Query";
            this.Description = "The query root of Snowflake's GraphQL interface";
        }
    }

    internal class RootMutation : ObjectGraphType<object>
    {
        public RootMutation()
        {
            this.Name = "Mutation";
            this.Description = "The mutation root of Snowflake's GraphQL interface";
        }
    }
}
