using HotChocolate;
using HotChocolate.Types;
using Snowflake.Remoting.GraphQL;
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
            descriptor.ExtendSubscription();
            descriptor.Field("onEmulationCleanup")
                .Description("A subscription for the cleanupEmulation mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationCleanupMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());

            descriptor.Field("onEmulationStart")
                .Description("A subscription for the startEmulation mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationStartMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());

            descriptor.Field("onEmulationStop")
                .Description("A subscription for when an emulation ends. The payload will be the same as in the corresponding `onEmulationStart` subscription.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationStopMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());
           
            descriptor.Field("onEmulationSetupEnvironment")
                .Description("A subscription for the setupEmulationEnvironment mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationSetupEnvironmentMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());

            descriptor.Field("onEmulationCompileConfiguration")
                .Description("A subscription for the compileEmulationConfiguration mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationCompileConfigurationMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());

            descriptor.Field("onEmulationRestoreSave")
                .Description("A subscription for the restoreEmulationSave mutation.")
                .Type<NonNullType<EmulationInstancePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationRestoreSaveMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<EmulationInstancePayload>());

            descriptor.Field("onEmulationPersistSave")
                .Description("A subscription for the persistEmulationSave mutation.")
                .Type<NonNullType<PersistEmulationSavePayloadType>>()
                .Argument("instanceId", arg => arg.Type<NonNullType<UuidType>>())
                .SubscribeToTopic<Guid, OnEmulationPersistSaveMessage>("instanceId")
                .Resolve(ctx => ctx.GetEventMessage<PersistEmulationSavePayload>());
        }
    }
}
