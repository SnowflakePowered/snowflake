using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class DeleteScrapeContextPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Task<IGame> Game { get; set; }
        public Guid JobID { get; set; }
        public bool Success { get; set; }
    }

    internal sealed class DeleteScrapeContextPayloadType
        : ObjectType<DeleteScrapeContextPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<DeleteScrapeContextPayload> descriptor)
        {
            descriptor.Name(nameof(DeleteScrapeContextPayload))
                .WithClientMutationId();
         
            descriptor.Field(c => c.JobID)
                .Name("jobId")
                .Description("The job GUID.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Description("The context that was deleted.")
                .Type<ScrapeContextType>();

            descriptor.Field(c => c.Success)
                .Name("success")
                .Description("Whether or not the job was removed from the queue")
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }

}
