using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing
{
    internal sealed class InstallationJobQueueType
        : ObjectType<IAsyncJobQueue<TaskResult<IFile>>>
    {
        protected override void Configure(IObjectTypeDescriptor<IAsyncJobQueue<TaskResult<IFile>>> descriptor)
        {
            descriptor.Name("InstallJobQueue")
                .Description("Provides access to values in the scraping job queue")
                .UseJobQueue();

            descriptor.Field("job")
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>()
                    .Description("The `jobId` of the installation job that can be used to query the installation state."))
                .Resolver(ctx =>
                {
                    var context = ctx.Parent<IAsyncJobQueue<TaskResult<IFile>>>();
                    return (context, ctx.Argument<Guid>("jobId"));
                })
                .Type<InstallationJobType>();
        }
    }
}
