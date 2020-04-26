using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports
{
    internal sealed class ClearPortDeviceInput
    {
        public string Orchestrator { get; set; }
        public int PortIndex { get; set; }
        public PlatformId PlatformID { get; set; }
    }

    internal sealed class ClearPortDeviceInputType
        : InputObjectType<ClearPortDeviceInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ClearPortDeviceInput> descriptor)
        {
            descriptor.Name(nameof(ClearPortDeviceInput));
            descriptor.Field(i => i.Orchestrator)
                .Description("The name of the emulator orchestrator that will have its set of virtual ports changed.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(i => i.PortIndex)
                .Description("The zero-indexed port index to update.")
                .Type<NonNullType<IntType>>();
            descriptor.Field(i => i.PlatformID)
                .Name("platformId")
                .Description("The platform ID of the set of ports that will be updated.")
                .Type<NonNullType<PlatformIdType>>();
        }
    }
}
