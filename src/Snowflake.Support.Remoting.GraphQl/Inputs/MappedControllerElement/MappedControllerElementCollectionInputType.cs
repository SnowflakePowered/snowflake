using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement
{
    public class MappedControllerElementCollectionInputType : InputObjectGraphType<MappedControllerElementCollectionInputObject>
    {
        public MappedControllerElementCollectionInputType()
        {
            Field(p => p.ControllerId);
            Field(p => p.DeviceId);
            Field(p => p.ProfileName);
            Field<ListGraphType<MappedControllerElementInputType>>("mappings",
                resolve: context => context.Source.Mappings);
        }
    }
}
