using HotChocolate.Types.Filters;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Stone
{
    public class ControllerLayoutFilter
        : FilterInputType<IControllerLayout>
    {
        protected override void Configure(IFilterInputTypeDescriptor<IControllerLayout> descriptor)
        {
            descriptor
                .Name("ControllerLayoutFilter")                
                .Filter(c => c.ControllerID)
                
                .AllowEquals()
                    .Name("controllerId")
                    .Description("Get the platform with the specific LayoutID.").And()
                .AllowIn()
                    .Name("controllerId_in")
                    .Description("Get platforms with the given LayoutIDs.").And()
                .AllowStartsWith()
                    .Name("controllerId_startsWith")
                    .Description("Get platforms whose LayoutIDs start with the given string");
        }
    }
}
