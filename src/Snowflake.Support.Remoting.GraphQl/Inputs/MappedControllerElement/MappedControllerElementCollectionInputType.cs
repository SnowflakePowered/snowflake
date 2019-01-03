using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.MappedControllerElement
{
    public class
        MappedControllerElementCollectionInputType : InputObjectGraphType<MappedControllerElementCollectionInputObject>
    {
        public MappedControllerElementCollectionInputType()
        {
            Name = "MappedControllerElementCollectionInput";
            Field(p => p.ControllerId);
            Field(p => p.DeviceId);
            Field(p => p.ProfileName);
            Field<ListGraphType<MappedControllerElementInputType>>("mappings",
                resolve: context => context.Source.Mappings);
        }
    }
}
