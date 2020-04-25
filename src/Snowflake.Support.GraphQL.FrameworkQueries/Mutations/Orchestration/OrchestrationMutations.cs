using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class OrchestrationMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Extend()
                .OnBeforeCreate(defn =>
                {
                    defn.ContextData.Add("Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache",
                        new ConcurrentDictionary<Guid, IGameEmulation>());
                });
            descriptor.Field("test")
                .Resolver(ctx =>
                {
                    return "no";
                }).Type<StringType>();
        }
    }

    static class OrchestrationMutationContextExtesions
    {
       internal static ConcurrentDictionary<Guid, IGameEmulation> GetGameCache(this IResolverContext ctx)
            => (ConcurrentDictionary<Guid, IGameEmulation>)ctx.RootType.ContextData["Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration.GameInstanceCache"];
    }
}
