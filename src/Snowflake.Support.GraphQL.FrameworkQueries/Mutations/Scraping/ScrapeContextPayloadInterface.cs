using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    public sealed class ScrapeContextPayloadInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("ScrapeContextPayload");

            descriptor.Field("context")
                .Type<ScrapeContextType>();
            descriptor.Field("jobId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field("game")
                .Type<NonNullType<GameType>>();
        }
    }
}
