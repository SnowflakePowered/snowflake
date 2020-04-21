using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Queueing
{
    public sealed class JobQueueInterface
        : InterfaceType<IAsyncJobQueue>
    {
        protected override void Configure(IInterfaceTypeDescriptor<IAsyncJobQueue> descriptor)
        {
            descriptor.Name("JobQueue")
                .Description("Provides queries for a job queue");

            descriptor.Field(s => s.GetActiveJobs())
                .Name("activeJobIds")
                .Description("The jobs currently active in the scraping queue.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
            descriptor.Field(s => s.GetQueuedJobs())
                .Name("queuedJobIds")
                .Description("The jobs currently in the scraping queue.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
            descriptor.Field(s => s.GetZombieJobs())
                .Name("zombieJobIds")
                .Description("The jobs that are still in the scraping queue, but no longer has items to process.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
        }
    }
}
