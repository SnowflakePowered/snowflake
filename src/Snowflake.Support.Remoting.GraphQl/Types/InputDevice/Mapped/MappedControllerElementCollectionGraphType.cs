using GraphQL.Types;
using Snowflake.Input.Controller.Mapped;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice.Mapped
{
    public class MappedControllerElementCollectionGraphType : ObjectGraphType<IMappedControllerElementCollection>
    {
        public MappedControllerElementCollectionGraphType()
        {
            Field(c => c.ControllerId).Description("The Stone Controller ID of the emulated controller this collection maps to.");
            Field(c => c.DeviceId).Description("The Controller ID of the real device this collection maps from.");
            Field<ListGraphType<MappedControllerElementGraphType>>("mappings",
                description: "The set of mappings that map each element from the real device to the emulated device.",
                resolve: context => context.Source);
        }
    }
}
