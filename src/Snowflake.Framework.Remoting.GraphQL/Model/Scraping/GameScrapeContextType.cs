using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Extensibility;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Scraping
{
    public sealed class GameScrapeContextType
        : ObjectType<IScrapeContext>
    {
        protected override void Configure(IObjectTypeDescriptor<IScrapeContext> descriptor)
        {
            descriptor.Name("ScrapeContext")
                .Description("Describes a transient context within which scraping occurs.");
            descriptor.Field(s => s.Cullers)
                .Description("The culler plugins that cull the resultant seed tree.")
                .Type<NonNullType<ListType<NonNullType<PluginType>>>>();
            descriptor.Field(s => s.Scrapers)
                .Description("The scraper plugins that contribute data to the seed tree.")
                .Type<NonNullType<ListType<NonNullType<PluginType>>>>();
            descriptor.Field(s => s.Context)
                .Description("The root seed context that contains the current state of the seed root tree.")
                .Type<NonNullType<SeedRootContextType>>();
        }
    }
}
