using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class DeleteScrapeContextInput
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
    }

    internal sealed class DeleteScrapeContextInputType
        : InputObjectType<DeleteScrapeContextInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteScrapeContextInput> descriptor)
        {
            descriptor.Name(nameof(DeleteScrapeContextInput))
                .WithClientMutationId();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
        }
    }
}
