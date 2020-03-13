using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{

    public class PlatformQueries
        : ObjectType<PlatformInfoQueryBuilder>
    {
        protected override void Configure(IObjectTypeDescriptor<PlatformInfoQueryBuilder> descriptor)
        {
            descriptor.Field(p => p.GetPlatforms())
                .UseFiltering()
                .Description("Gets the Stone Platforms definitions matching the search query.");
        }
    }

    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var rootSchema = coreInstance.Get<IGraphQLService>();
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();

      
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            
            hotChocolate.AddQuery<PlatformQueries, PlatformInfoQueryBuilder>(platformQueries);

            //rootSchema.Register(platformQueries);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Platform GraphQL Queries.");
        }
    }
}
