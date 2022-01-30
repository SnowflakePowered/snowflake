using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Orchestration
{
    public sealed class GameEmulationType
        : ObjectType<IGameEmulation>
    {
        protected override void Configure(IObjectTypeDescriptor<IGameEmulation> descriptor)
        {
            descriptor.Name("GameEmulation")
                .Description("Describes an instance of a game being emulated.");
            descriptor.Field(e => e.SaveProfile)
                .Description("The save profile being used for this emulation.")
                .Type<NonNullType<SaveProfileType>>();
            descriptor.Field(e => e.Game)
                .Description("The game being emulated.")
                .Type<NonNullType<GameType>>();
            descriptor.Field(e => e.ControllerPorts)
                .Description("The input devices and their associations to the emulated input devices being used for this emulation.")
                .Type<NonNullType<ListType<NonNullType<EmulatedControllerType>>>>();
            descriptor.Field(e => e.EmulationState)
                .Description("The current state of the emulation.")
                .Type<NonNullType<GameEmulationStateEnum>>();
            descriptor.Field(e => e.Guid)
                .Description("The GUID of the game emulation instance.")
                .Type<NonNullType<UuidType>>();
        }
    }
}
