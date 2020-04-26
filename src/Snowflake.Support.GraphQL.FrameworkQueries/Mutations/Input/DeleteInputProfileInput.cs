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
    internal sealed class DeleteInputProfileInput
    {
        public Guid ProfileID { get; set; }
    }

    internal sealed class DeleteInputProfileInputType
        : InputObjectType<DeleteInputProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteInputProfileInput> descriptor)
        {
            descriptor.Field(i => i.ProfileID)
                .Name("profileId")
                .Description("The GUID of the input profile to delete.");
        }
    }
}
