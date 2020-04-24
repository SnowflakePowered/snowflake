using HotChocolate.Types;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Installation.Tasks;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation;
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

            descriptor.Field("createValidation")
                .Description("Creates a validation intallation with the specified game and orchestrator.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<CreateValidationInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<CreateValidationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var orchestrator = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IEmulatorOrchestrator>()[arg.Orchestrator];

                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (orchestrator == null) throw new ArgumentException("The specified orchestrator plugin is not installed.");
                    if (game == null) throw new ArgumentException("The specified game was not found.");
                    var compatibility = orchestrator.CheckCompatibility(game);
                    if (compatibility != EmulatorCompatibility.RequiresValidation)
                        throw new ArgumentException("The specified orchestrator can not validate the specified game. Either it is unsupported, " +
                            "or has already been validated.");
                    var jobId = await jobQueue.QueueJob(orchestrator.ValidateGamePrerequisites(game));
                    ctx.AssignGameGuid(game, jobId);
                    return new CreateValidationPayload
                    {
                        JobID = jobId,
                        Game = game,
                    };
                })
                .Type<NonNullType<CreateValidationPayloadType>>();

            descriptor.Field("createInstallation")
                .Description("Creates a new installation with the specified artifacts and game.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<CreateInstallationInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<CreateInstallationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var installer = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IGameInstaller>()[arg.Installer];

                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (installer == null) throw new ArgumentException("The specified installer plugin is not installed.");
                    if (game == null) throw new ArgumentException("The specified game was not found.");
                    var jobId = await jobQueue.QueueJob(installer.Install(game, arg.Artifacts));
                    ctx.AssignGameGuid(game, jobId);
                    return new CreateInstallationPayload
                    {
                        JobID = jobId,
                        Game = game,
                    };
                })
                .Type<NonNullType<CreateInstallationPayloadType>>();

            descriptor.Field("nextInstallationStep")
                .Description("Proceeds with the next step in the installation. If an exception occurs, cancellation will automatically be requested.")
                .UseClientMutationId()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<NextInstallationStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var (newFile, moved) = await jobQueue.GetNext(arg.JobID);

                    // Force execution of task in case it wasn't already executed.
                    if (newFile.Error == null)
                    {
                        await newFile;
                    }
                    else
                    {
                        jobQueue.RequestCancellation(arg.JobID);
                        await ctx.SendEventMessage(new OnInstallationCancelledMessage(arg.JobID, new InstallationCancelledPayload()
                        {
                            Game = ctx.GetAssignedGame(arg.JobID),
                            JobID = arg.JobID,
                            ClientMutationID = arg.ClientMutationID,
                        }));
                    }

                    if (moved)
                    {
                        var payload = new InstallationStepPayload()
                        {
                            JobID = arg.JobID,
                            Current = newFile,
                            Game = ctx.GetAssignedGame(arg.JobID),
                        };
                        await ctx.SendEventMessage(new OnInstallationStepMessage(arg.JobID, payload));
                        return payload;
                    }
                    var finishedPayload = new InstallationCompletePayload()
                    {
                        JobID = arg.JobID,
                        Game = ctx.GetAssignedGame(arg.JobID).ContinueWith(g =>
                        {
                            ctx.RemoveAssignment(arg.JobID);
                            return g;
                        }).Unwrap(),
                    };
                    await ctx.SendEventMessage(new OnInstallationCompleteMessage(arg.JobID, finishedPayload));
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationPayloadInterface>>();

            descriptor.Field("exhaustInstallationSteps")
                .Description("Exhaust all steps in the installation. " +
                "If an error occurs during installation, cancellation will automatically be requested, but the " +
                "installation must run to completion.")
                .UseClientMutationId()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<NextInstallationStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();
                    var gameTask = ctx.GetAssignedGame(arg.JobID);

                    await foreach (TaskResult<IFile> newFile in jobQueue.AsEnumerable(arg.JobID))
                    {
                        var payload = new InstallationStepPayload()
                        {
                            JobID = arg.JobID,
                            Current = newFile,
                            Game = gameTask,
                            ClientMutationID = arg.ClientMutationID,
                        };

                        if (newFile.Error == null)
                        {
                            await newFile;
                            await ctx.SendEventMessage(new OnInstallationStepMessage(arg.JobID, payload));
                        }
                        else
                        {
                            jobQueue.RequestCancellation(arg.JobID);
                            await ctx.SendEventMessage(new OnInstallationStepMessage(arg.JobID, payload));
                            await ctx.SendEventMessage(new OnInstallationCancelledMessage(arg.JobID, new InstallationCancelledPayload()
                            {
                                Game = gameTask,
                                JobID = arg.JobID,
                                ClientMutationID = arg.ClientMutationID,
                            }));

                        }
                    }

                    var finishedPayload = new InstallationCompletePayload()
                    {
                        ClientMutationID = arg.ClientMutationID,
                        JobID = arg.JobID,
                        Game = gameTask
                            .ContinueWith(g =>
                            {
                                ctx.RemoveAssignment(arg.JobID);
                                return g;
                            }).Unwrap(),
                    };
                    await ctx.SendEventMessage(new OnInstallationCompleteMessage(arg.JobID, finishedPayload));
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationCompletePayloadType>>();

            descriptor.Field("cancelInstallation")
                .Description("Requests cancellation of an installation. This does not mean the installation step is complete. The installation" +
                " must be continued to ensure proper cleanup.")
                .UseAutoSubscription()
                .UseClientMutationId()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<NextInstallationStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    jobQueue.RequestCancellation(arg.JobID);

                    var finishedPayload = new InstallationCancelledPayload()
                    {
                        JobID = arg.JobID,
                        Game = ctx.GetAssignedGame(arg.JobID)
                    };
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationCancelledPayloadType>>();
        }
    }
}
