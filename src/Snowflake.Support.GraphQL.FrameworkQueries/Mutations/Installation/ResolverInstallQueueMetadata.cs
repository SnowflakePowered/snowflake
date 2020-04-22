using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal static class ResolverInstallQueueMetadata
    {
        public static IResolverContext AssignGameGuid(this IResolverContext context, IGame game, Guid jobGuid)
        {
            ((ConcurrentDictionary<Guid, Guid>)context
                .ContextData["Snowflake.Support.GraphQL.FrameworkQueries.ResolverJobQueueMetadata.Store"])
                .TryAdd(jobGuid, game.Record.RecordID);
            return context;
        }

        public static Task<IGame?> GetAssignedGame(this IResolverContext context, Guid jobGuid)
        {
            ((ConcurrentDictionary<Guid, Guid>)context
                .ContextData["Snowflake.Support.GraphQL.FrameworkQueries.ResolverJobQueueMetadata.Store"])
                .TryGetValue(jobGuid, out Guid gameGuid);
            if (gameGuid == default) return Task.FromResult<IGame?>(null);
            return context.SnowflakeService<IGameLibrary>().GetGameAsync(gameGuid);
        }
    }
}
