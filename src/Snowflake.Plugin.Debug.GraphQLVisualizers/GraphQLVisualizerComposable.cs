using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Remoting.Kestrel;
using Snowflake.Loader;
using GraphQL.Server.Ui.Altair;

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
            app.UseGraphQLGraphiQL(path: "/debug/gql/graphiql");
            app.UseGraphQLVoyager(path: "/debug/gql/voyager");
            app.UseGraphQLAltair(path: "/debug/gql/altair");
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
