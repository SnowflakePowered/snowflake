using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class TestQuery
        //: ObjectType<PlatformInfoQueryBuilder>
    {
        //protected override void Configure(IObjectTypeDescriptor<PlatformInfoQueryBuilder> descriptor)
        //{
        //    descriptor.Field(p => p.GetPlatform(default(PlatformId)))
        //        .Argument("platformID", a => a.Description("platform Id"));
        //}

        public IPlatformInfo GetPlatform([Service] IStoneProvider p, PlatformId platformId)
        {
            return p.Platforms[platformId];
        }

        /// <summary>
        /// Testing One Two Three
        /// </summary>
        /// <returns>Hello World</returns>
        public PlatformId Hello() => "Hello";

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

            hotChocolate.RegisterSchema(coreInstance, "snowflake", "platforms", schema =>
            {
                schema.AddQueryType<TestQuery>();
            });

            var platformQueries = new PlatformInfoQueryBuilder(stone);
            rootSchema.Register(platformQueries);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Platform GraphQL Queries.");
        }
    }
}
