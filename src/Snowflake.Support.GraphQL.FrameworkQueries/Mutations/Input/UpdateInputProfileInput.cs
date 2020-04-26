using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    internal sealed class UpdateInputProfileInput
    {
        public Guid ProfileID { get; set; }
        public List<ControllerElementMappingInput> Mappings { get; set; }
    }

    internal sealed class UpdateInputProfileInputType
        : InputObjectType<UpdateInputProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateInputProfileInput> descriptor)
        {
            descriptor.Field(i => i.ProfileID)
                .Name("profileId")
                .Description("The profile GUID of the input profile to update.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Mappings)
                .Description("The updated mappings to apply.")
                .Type<NonNullType<ListType<NonNullType<ControllerElementMappingInputType>>>>();
        }
    }
}
