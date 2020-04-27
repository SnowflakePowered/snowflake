using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;
using Snowflake.Model.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using System.Linq;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal sealed class InstallationSubscriptions
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Subscription");

            descriptor.Field("onInstallationStep")
                .Description("A subscription for when a game installation step occurs.")
                .Type<NonNullType<InstallationStepPayloadType>>()
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnInstallationStepMessage>();
                    return message.Payload;
                });
            descriptor.Field("onInstallationComplete")
                .Description("A subscription for when a game installation completes.")
                .Type<NonNullType<InstallationCompletePayloadType>>()
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnInstallationCompleteMessage>();
                    return message.Payload;
                });
            descriptor.Field("onInstallationCancel")
                .Description("A subscription for when a game installation is cancelled.")
               .Type<NonNullType<InstallationCancelledPayloadType>>()
               .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
               .Resolver(ctx =>
               {
                   var message = ctx.GetEventMessage<OnInstallationCancelMessage>();
                   return message.Payload;
               });
        }
    }
}
