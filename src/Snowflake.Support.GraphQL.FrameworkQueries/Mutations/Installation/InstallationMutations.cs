using HotChocolate.Types;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    public sealed class InstallationMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createInstallation")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<CreateInstallationInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<CreateInstallationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var installer = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IGameInstaller>()
                        .FirstOrDefault(c => String.Equals(c.Name, arg.Installer, StringComparison.InvariantCultureIgnoreCase));
                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (installer == null) throw new ArgumentException("The specified installer plugin is not installed.");
                    if (game == null) throw new ArgumentException("The specified game was not found.");
                    var jobId = await jobQueue.QueueJob(installer.Install(game, arg.Artifacts));
                    ctx.AssignGameGuid(game, jobId);
                    return new CreateInstallationPayload
                    {
                        JobID = jobId,
                        Game = Task<IGame>.FromResult(game),
                    };
                })
                .Type<NonNullType<CreateInstallationPayloadType>>();

            descriptor.Field("nextInstallationStep")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<NextInstallationStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    //var (newFile, moved) = await jobQueue.GetNext(arg.JobID);
                    var game = await ctx.GetAssignedGame(arg.JobID);

                    return "Test";
                })
                .Type<StringType>();
        }
    }
}
