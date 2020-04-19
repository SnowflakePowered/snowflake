using HotChocolate.Types;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Scraping
{
    public sealed class SeedRootContextType
        : ObjectType<ISeedRootContext>
    {
        protected override void Configure(IObjectTypeDescriptor<ISeedRootContext> descriptor)
        {
            descriptor.Name("SeedRootContext")
                .Description("The context root of a scraping seed tree");

            descriptor.Field(s => s.Root)
                .Description("The root seed")
                .Type<SeedType>();

            descriptor.Field("descendants")
                .Description("The unculled descendants of a given seed. " +
                "If the seed does not exist, returns the empty array. " +
                "If no seed is given, returns the descendants of the root.")
                .Argument("seedId", arg => arg.Type<UuidType>())
                .Resolver(ctx =>
                {
                    var seedRootContext = ctx.Parent<ISeedRootContext>();
                    Guid seedGuid = ctx.Argument<Guid>("seedId");
                    if (seedGuid == default) seedGuid = seedRootContext.Root.Guid;

                    var seed = seedRootContext[seedGuid];
                    return seedRootContext.GetDescendants(seed);
                })
                .Type<NonNullType<ListType<NonNullType<SeedType>>>>();

            descriptor.Field("siblings")
                .Description("The unculled siblings of a given seed. If the seed does not exist, returns the empty array.")
                .Argument("seedId", arg => arg.Type<UuidType>())
                .Resolver(ctx =>
                {
                    var seedRootContext = ctx.Parent<ISeedRootContext>();
                    Guid seedGuid = ctx.Argument<Guid>("seedId");
                    if (seedGuid == default) seedGuid = seedRootContext.Root.Guid;

                    var seed = seedRootContext[seedGuid];
                    return seedRootContext.GetSiblings(seed);
                })
                .Type<NonNullType<ListType<NonNullType<SeedType>>>>();

            descriptor.Field("children")
                .Description("The direct children of a given seed. If the seed does not exist, returns an empty array.")
                .Argument("seedId", arg => arg.Type<UuidType>())
                .Resolver(ctx =>
                {
                    var seedRootContext = ctx.Parent<ISeedRootContext>();
                    Guid seedGuid = ctx.Argument<Guid>("seedId");
                    if (seedGuid == default) seedGuid = seedRootContext.Root.Guid;

                    var seed = seedRootContext[seedGuid];
                    return seedRootContext.GetChildren(seed);
                })
                .Type<NonNullType<ListType<NonNullType<SeedType>>>>();

            descriptor.Field("allSeeds")
                .Description("All seeds, whether culled or unculled in the tree.")
                .Resolver(ctx =>
                {
                    Guid seedGuid = ctx.Argument<Guid>("seedId");
                    var seedRootContext = ctx.Parent<ISeedRootContext>();
                    return seedRootContext.GetAll();
                })
                .Type<NonNullType<ListType<SeedType>>>();
        }
    }
}
