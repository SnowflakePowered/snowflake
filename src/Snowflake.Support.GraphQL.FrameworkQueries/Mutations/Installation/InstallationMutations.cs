using HotChocolate;
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
            descriptor.ExtendMutation();

            descriptor.Field("createValidation")
                .Description("Creates a validation intallation with the specified game and orchestrator.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<CreateValidationInputType>())
                .Resolve(async ctx =>
                {
                    var arg = ctx.ArgumentValue<CreateValidationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var orchestrator = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IEmulatorOrchestrator>()[arg.Orchestrator];

                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (orchestrator == null) 
                        return ErrorBuilder.New()
                                .SetCode("INST_NOTFOUND_ORCHESTRATOR")
                                .SetMessage("The specified orchestrator was not found.")
                                .Build();
                    if (game == null) 
                        return ErrorBuilder.New()
                                    .SetCode("INST_NOTFOUND_GAME")
                                    .SetMessage("The specified game does not exist.")
                                    .Build();
                    var compatibility = orchestrator.CheckCompatibility(game);
                    if (compatibility != EmulatorCompatibility.RequiresValidation)
                        return ErrorBuilder.New()
                                        .SetCode("INST_ORCH_CANNOTVALIDATE")
                                        .SetMessage("The specified orchestrator can not validate the specified game. Either it is unsupported, " +
                                                    "or has already been validated.")
                                        .SetExtension("compatibility", compatibility.ToString())
                                        .Build();
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
                .Resolve(async ctx =>
                {
                    var arg = ctx.ArgumentValue<CreateInstallationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    var installer = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IGameInstaller>()[arg.Installer];

                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (installer == null) 
                        return ErrorBuilder.New()
                                    .SetCode("INST_NOTFOUND_INSTALLER")
                                    .SetMessage("The specified installer was not found.")
                                    .Build();
                    if (game == null) 
                        return ErrorBuilder.New()
                                    .SetCode("INST_NOTFOUND_GAME")
                                    .SetMessage("The specified game does not exist.")
                                    .Build();
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
                .UseAutoSubscription()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolve(async ctx =>
                {
                    var arg = ctx.ArgumentValue<NextInstallationStepInput>("input");
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
                        ctx.ReportError(ErrorBuilder.New()
                            .SetException(newFile.Error)
                            .SetCode("INST_ERR_INSTALL")
                            .Build());
                        jobQueue.RequestCancellation(arg.JobID);
                        await ctx.SendEventMessage(new OnInstallationCancelMessage(
                            new InstallationCancelledPayload()
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
                            ClientMutationID = arg.ClientMutationID,
                            JobID = arg.JobID,
                            Current = newFile,
                            Game = ctx.GetAssignedGame(arg.JobID),
                        };
                        await ctx.SendEventMessage(new OnInstallationStepMessage(payload));
                        return payload;
                    }
                    var finishedPayload = new InstallationCompletePayload()
                    {
                        ClientMutationID = arg.ClientMutationID,
                        JobID = arg.JobID,
                        Game = ctx.GetAssignedGame(arg.JobID).ContinueWith(g =>
                        {
                            ctx.RemoveAssignment(arg.JobID);
                            return g;
                        }).Unwrap(),
                    };
                    await ctx.SendEventMessage(new OnInstallationCompleteMessage(finishedPayload));
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationPayloadInterface>>();

            descriptor.Field("exhaustInstallationSteps")
                .Description("Exhaust all steps in the installation. " +
                "If an error occurs during installation, cancellation will automatically be requested, but the " +
                "installation must run to completion.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolve(async ctx =>
                {
                    var arg = ctx.ArgumentValue<NextInstallationStepInput>("input");
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
                            await ctx.SendEventMessage(new OnInstallationStepMessage(payload));
                        }
                        else
                        {
                            jobQueue.RequestCancellation(arg.JobID);
                            await ctx.SendEventMessage(new OnInstallationStepMessage(payload));
                            await ctx.SendEventMessage(new OnInstallationCancelMessage(
                                new InstallationCancelledPayload()
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
                    await ctx.SendEventMessage(new OnInstallationCompleteMessage(finishedPayload));
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationCompletePayloadType>>();

            descriptor.Field("cancelInstallation")
                .Description("Requests cancellation of an installation. This does not mean the installation step is complete. The installation" +
                " must be continued to ensure proper cleanup.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", a => a.Type<NextInstallationStepInputType>())
                .Resolve(async ctx =>
                {
                    var arg = ctx.ArgumentValue<NextInstallationStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();

                    jobQueue.RequestCancellation(arg.JobID);

                    var finishedPayload = new InstallationCancelledPayload()
                    {
                        ClientMutationID = arg.ClientMutationID,
                        JobID = arg.JobID,
                        Game = ctx.GetAssignedGame(arg.JobID)
                    };

                    await ctx.SendEventMessage(new OnInstallationCancelMessage(finishedPayload));
                    return finishedPayload;
                })
                .Type<NonNullType<InstallationCancelledPayloadType>>();
        }
    }
}
