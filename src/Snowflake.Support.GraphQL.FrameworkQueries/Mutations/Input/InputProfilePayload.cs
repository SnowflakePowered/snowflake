using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Device.Mapped;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    public sealed class InputProfilePayload
        : RelayMutationBase
    {
        public IControllerElementMappingProfile InputProfile { get; set; }
    }

    public sealed class InputProfilePayloadType
        : ObjectType<InputProfilePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<InputProfilePayload> descriptor)
        {
            descriptor.Name("InputProfilePayload")
                .WithClientMutationId();
            descriptor.Field(i => i.InputProfile)
                .Description("The input profile that was modified.")
                .Type<NonNullType<ControllerElementMappingProfileType>>();
        }
    }
}
