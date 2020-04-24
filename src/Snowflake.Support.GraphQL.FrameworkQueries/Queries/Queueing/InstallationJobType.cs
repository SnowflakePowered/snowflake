using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Remoting.GraphQL.Model.Installation.Tasks;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing
{
    internal sealed class InstallationJobType
        : ObjectType<(IAsyncJobQueue<TaskResult<IFile>>, Guid)>
    {
        protected override void Configure(IObjectTypeDescriptor<(IAsyncJobQueue<TaskResult<IFile>>, Guid)> descriptor)
        {
            descriptor.Name("InstallationJob")
                .Description("Describes a single scraping job.")
                .Interface<QueuableJobInterface>();
            descriptor.Field("jobId")
              .Type<NonNullType<UuidType>>()
              .Resolver(ctx =>
              {
                  var (_, token) = ctx.Parent<(IAsyncJobQueue<TaskResult<IFile>>, Guid)>();
                  return token;
              });
            descriptor.Field("current")
                .Type<TaskResultType<IFile, ContextualFileInfoType>>()
                .Resolver(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<TaskResult<IFile>>, Guid)>();
                    return queue.GetCurrent(token);
                });
            descriptor.Field("context")
                .Type<ScrapeContextType>()
                .Resolver(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<TaskResult<IFile>>, Guid)>();
                    return queue.GetSource(token);
                });
            descriptor.Field("game")
                .Description("The game associated with this job, if any.")
                .Type<GameType>()
                .Resolver(ctx =>
                {
                    var (_, token) = ctx.Parent<(IAsyncJobQueue<TaskResult<IFile>>, Guid)>();
                    return ctx.GetAssignedGame(token);
                });
        }
    }
}
