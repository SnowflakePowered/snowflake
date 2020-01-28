using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement
{
    public class
        DefaultMappedControllerElementCollectionInputType : InputObjectGraphType<
            DefaultMappedControllerElementCollectionInputObject>
    {
        public DefaultMappedControllerElementCollectionInputType()
        {
            Name = "DefaultMappedControllerElementCollectionInput";
            Field(p => p.ControllerId);
            Field(p => p.InputDriver);
            Field(p => p.InstanceGuid);
            Field(p => p.ProfileName).DefaultValue("default");
        }
    }
}
