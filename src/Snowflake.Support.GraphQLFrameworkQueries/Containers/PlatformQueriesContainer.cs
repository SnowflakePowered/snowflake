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
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Filesystem;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Game;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Installables;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.LibraryExtensions;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
          
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();

            hotChocolate.AddObjectTypeExtension<StoneQueries>();
            hotChocolate.AddObjectTypeExtension<GameQueries>();
            hotChocolate.AddObjectTypeExtension<GameFileQueries>();
            hotChocolate.AddObjectTypeExtension<GameNodeQueries>();
            hotChocolate.AddObjectTypeExtension<EchoQueries>();
            hotChocolate.AddObjectTypeExtension<DeviceQueries>();
            hotChocolate.AddObjectTypeExtension<DeviceNodeQueries>();
            hotChocolate.AddObjectTypeExtension<PlatformInfoNodeQueries>();
            hotChocolate.AddObjectTypeExtension<ControllerLayoutNodeQueries>();

            hotChocolate.AddObjectTypeExtension<RuntimeQueries>();
            hotChocolate.AddObjectTypeExtension<FilesystemQueries>();

            hotChocolate.AddInterfaceTypeExtension<InstallableQueries>();
            hotChocolate.AddObjectTypeExtension<DirectoryInfoInstallableQueries>();
            hotChocolate.AddObjectTypeExtension<DriveInfoInstallableQueries>();

            hotChocolate.AddObjectTypeExtension<InputDeviceInstanceMappingProfileQueries>();
            hotChocolate.AddObjectTypeExtension<InputDeviceMappingProfilesQueries>();
            hotChocolate.AddObjectTypeExtension<ControllerElementMappingProfileNodeQueries>();

            hotChocolate.AddObjectTypeExtension<SaveQueries>();

            //hotChocolate.AddQuery<PlatformQueries, PlatformInfoQueryBuilder>(platformQueries);
            //hotChocolate.AddObjectTypeExtension<GameRecordQueries>();
            //hotChocolate.AddQuery<GameQueries, GameQueryBuilder>(gameQueries);
        }
    }
}
