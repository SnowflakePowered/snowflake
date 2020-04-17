using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.PortEntry
{
    public class PortEntryInputType : InputObjectGraphType<PortEntryInputObject>
    {
        public PortEntryInputType()
        {
            Name = "PortEntryInput";

            Field<NonNullGraphType<GuidGraphType>>("instanceGuid",
              resolve: context => context.Source.InstanceGuid,
              description: "The instance GUID of the device.");
            Field<NonNullGraphType<InputDriverEnum>>("instanceDriver",
              resolve: context => context.Source.InstanceDriver,
              description: "The driver of the device instance.");
            Field<NonNullGraphType<StringGraphType>>("mappingsProfile",
                resolve: context => context.Source.MappingsProfile,
                description: "The controller mappings profile to apply to this device.");
            Field<NonNullGraphType<StringGraphType>>("controllerId",
                  resolve: context => context.Source.ControllerId,
                  description: "The Stone controller ID that this port entry is set to");
        }
    }
}
