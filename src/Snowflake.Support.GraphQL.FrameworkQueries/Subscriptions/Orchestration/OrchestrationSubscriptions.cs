using HotChocolate.Types;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Orchestration
{
    public sealed class OrchestrationSubscriptions
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Subscription");
            descriptor.Field("onEmulationCleanup")
                .Description("A subscription for the cleanupEmulation mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationCleanup>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationStart")
                .Description("A subscription for the startEmulation mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationStart>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationStop")
                .Description("A subscription for when an emulation ends. The payload will be the same as in the corresponding `onEmulationStart` subscription.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationStop>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationSetupEnvironment")
                .Description("A subscription for the setupEmulationEnvironment mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationSetupEnvironment>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationCompileConfiguration")
                .Description("A subscription for the compileEmulationConfiguration mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationCompileConfiguration>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationRestoreSave")
                .Description("A subscription for the restoreEmulationSave mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationRestoreSave>();
                    return message.Payload;
                });
            descriptor.Field("onEmulationPersistSave")
                .Description("A subscription for the persistEmulationSave mutation.")
                .Type<NonNullType<PersistEmulationSavePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnEmulationPersistSave>();
                    return message.Payload;
                });
        }
    }
}
