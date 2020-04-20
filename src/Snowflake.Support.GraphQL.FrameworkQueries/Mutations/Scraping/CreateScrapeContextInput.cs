using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class CreateScrapeContextInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public IList<string> Cullers { get; set; }
        public IList<string> Scrapers { get; set; }
    }

    internal sealed class CreateScrapeContextInputType
        : InputObjectType<CreateScrapeContextInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateScrapeContextInput> descriptor)
        {
            descriptor.Name(nameof(CreateScrapeContextInput))
                .WithClientMutationId();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.Scrapers)
                .Name("scrapers")
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();

            descriptor.Field(i => i.Cullers)
                .Name("cullers")
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }

}
