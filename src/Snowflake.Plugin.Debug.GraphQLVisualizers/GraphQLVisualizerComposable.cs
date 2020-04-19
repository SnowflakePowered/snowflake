using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Remoting.Kestrel;
using Snowflake.Loader;

namespace Snowflake.Plugin.Debug.GraphQLVisualizers
{
    public class GraphQLVisualizerComposable : IComposable
    {
        [ImportService(typeof(IKestrelWebServerService))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IKestrelWebServerService>()?.AddService(new GraphQLVisualizerService());
        }
    }

    internal class GraphQLVisualizerService : IKestrelServerMiddlewareProvider
    {
        public void Configure(IApplicationBuilder app)
        {
            // use graphiQL middleware at default url /graphiql
            app.UseGraphiQLServer(new GraphiQLOptions() { Path = "/debug/gql/graphiql" });

            // use graphql-playground middleware at default url /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions() { Path = "/debug/gql/playground" });

            // use voyager middleware at default url /ui/voyager
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions() { Path = "/debug/gql/voyager" });

            // use voyager middleware at default url /ui/voyager
            app.UseGraphQLAltair(new GraphQLAltairOptions() { Path = "/debug/gql/altair" });
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
