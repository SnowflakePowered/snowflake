using HotChocolate.Types;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Scraping
{
    public sealed class SeedType
        : ObjectType<ISeed>
    {
        protected override void Configure(IObjectTypeDescriptor<ISeed> descriptor)
        {
            descriptor.Name("Seed")
                .Description("Represents a unit of scraped content.");
            descriptor.Field(s => s.Guid)
                .Name("seedId")
                .Description("A unique GUID that identifies this seed within a specific scrape context.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(s => s.Content)
                .Description("The scraped contents of this seed.")
                .Type<NonNullType<SeedContentType>>();
            descriptor.Field(s => s.Source)
                .Description("The source of the seed's contents, or where the content was scraped from. If this data was supplied manually," +
                " this is the string `__client`");
            descriptor.Field(s => s.Parent)
                .Name("parentSeedId")
                .Description("The seedId of the seed's parent seed within a specific scrape context.");
        }
    }
}
