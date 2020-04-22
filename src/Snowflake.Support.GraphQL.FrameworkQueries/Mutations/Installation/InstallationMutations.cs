using HotChocolate.Types;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                .Argument("input", a => a.Type<CreateInstallationInputType>())
                .Resolver(async ctx =>
                {
                    var arg = ctx.Argument<CreateInstallationInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();
                    var installer = ctx.SnowflakeService<IPluginCollection<IGameInstaller>>()
                        .FirstOrDefault(c => String.Equals(c.Name, arg.Installer, StringComparison.InvariantCultureIgnoreCase));
                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(arg.GameID);
                    if (installer == null) throw new ArgumentException("The specified installer plugin is not installed.");
                    if (game == null) throw new ArgumentException("The specified game was not found.");
                    new CreateInstallationPayload
                    {
                        JobID = await jobQueue.QueueJob(installer.Install(game, arg.Artifacts))
                    };
                })
                .Type<NonNullType<CreateInstallationPayloadType>>();
        }
    }
}
