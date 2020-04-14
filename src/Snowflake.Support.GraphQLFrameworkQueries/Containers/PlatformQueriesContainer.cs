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
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Debug;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Filesystem;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Game;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.LibraryExtensions;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
          
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();

            hotChocolate.AddObjectTypeExtension<PlatformQueries>();
            hotChocolate.AddObjectTypeExtension<GameQueries>();
            hotChocolate.AddObjectTypeExtension<GameRecordQueries>();
            hotChocolate.AddObjectTypeExtension<GameFileQueries>();
            hotChocolate.AddObjectTypeExtension<GameNodeQueries>();
            hotChocolate.AddObjectTypeExtension<EchoQueries>();
            hotChocolate.AddObjectTypeExtension<FilesystemQueries>();

            //hotChocolate.AddQuery<PlatformQueries, PlatformInfoQueryBuilder>(platformQueries);
            //hotChocolate.AddObjectTypeExtension<GameRecordQueries>();
            //hotChocolate.AddQuery<GameQueries, GameQueryBuilder>(gameQueries);
        }
    }
}
