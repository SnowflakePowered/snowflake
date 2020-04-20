using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class SeedTreeGraftInput
    {
        public Guid SeedID { get; set; }
        public SeedTreeInput Tree { get; set; }
    }

    internal sealed class SeedTreeGraftInputType
        : InputObjectType<SeedTreeGraftInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SeedTreeGraftInput> descriptor)
        {
            descriptor.Name(nameof(SeedTreeGraftInput))
                .Description("Describes a graft onto a seed tree.");
            descriptor.Field(s => s.SeedID)
                .Name("seedId")
                .Description("The GUID of the seed on which to graft the provided tree as a child.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(s => s.Tree)
                .Description("The tree of seeds to graft as a child onto the specified seed. ")
                .Type<NonNullType<SeedTreeInputType>>();
        }
    }
}
