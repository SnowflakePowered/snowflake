using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice.Mapped
{
    public class MappedControllerElementCollectionGraphType : ObjectGraphType<IControllerElementMappingProfile>
    {
        public MappedControllerElementCollectionGraphType()
        {
            Name = "ControllerElementMappings";
            Description =
                "A collection of mapped controller elements. Essentially a mapping profile from a real device to an emulated virtual device.";
            Field<StringGraphType>("controllerId", resolve: c => c.Source.ControllerID, description: "The Stone Controller ID of the emulated controller this collection maps to.");
            Field(c => c.DeviceName).Description("The Controller ID of the real device this collection maps from.");
            Field<ListGraphType<MappedControllerElementGraphType>>("mappings",
                description: "The set of mappings that map each element from the real device to the emulated device.",
                resolve: context => context.Source);
        }
    }
}
