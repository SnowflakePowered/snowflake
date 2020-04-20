using GraphQL.Types;
using HotChocolate.Types;
using Snowflake.Scraping.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class SeedTreeInput
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public IEnumerable<SeedTreeInput> Children { get; set; }
    }

    internal sealed class SeedTreeInputType
        : InputObjectType<SeedTreeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SeedTreeInput> descriptor)
        {
            descriptor.Name("SeedTreeInput")
                .Description("Describes an input tree of seeds to be added to a scrape context.");
            descriptor.Field(s => s.Type)
                .Description("The type of the seed node.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Value)
                .Description("The value of the seed node.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Children)
                .Description("Any children that are relevant to this node of the seed tree.")
                .Type<ListType<NonNullType<SeedTreeInputType>>>();
        }
    }
    internal static class SeedTreeInputExtensions
    {
        // todo: use a queue
        public static SeedTree ToSeedTree(this SeedTreeInput @this)
        {
            return (@this.Type, @this.Value,
                @this.Children?.Select(s => s.ToSeedTree()) ?? Enumerable.Empty<SeedTree>());
        }
    }
}
