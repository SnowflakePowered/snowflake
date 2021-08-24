using HotChocolate.Data.Filters;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public class ControllerLayoutFilterInputType
        : FilterInputType<IControllerLayout>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IControllerLayout> descriptor)
        {
            descriptor
                .Name("ControllerLayoutFilter")
                .Field(c => c.ControllerID)
                    .Name("controllerId");
                
                //.AllowEquals()
                //    .Name("controllerId")
                //    .Description("Get the platform with the specific LayoutID.").And()
                //.AllowIn()
                //    .Name("controllerId_in")
                //    .Description("Get platforms with the given LayoutIDs.").And()
                //.AllowStartsWith()
                //    .Name("controllerId_startsWith")
                //    .Description("Get platforms whose LayoutIDs start with the given string");
        }
    }
}
