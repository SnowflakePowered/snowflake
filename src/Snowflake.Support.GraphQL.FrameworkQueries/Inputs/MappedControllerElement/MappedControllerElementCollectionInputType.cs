using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement
{
    public class
        MappedControllerElementCollectionInputType : InputObjectGraphType<MappedControllerElementCollectionInputObject>
    {
        public MappedControllerElementCollectionInputType()
        {
            Name = "MappedControllerElementCollectionInput";
            Field(p => p.ControllerId);
            Field(p => p.DeviceName);
            Field(p => p.InputDriver);
            Field(p => p.VendorID);
            Field(p => p.ProfileName);
            Field<ListGraphType<MappedControllerElementInputType>>("mappings",
                resolve: context => context.Source.Mappings);
        }
    }
}
