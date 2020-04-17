using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement
{
    public class MappedControllerElementInputType : InputObjectGraphType<MappedControllerElementInputObject>
    {
        public MappedControllerElementInputType()
        {
            Name = "MappedControllerElementInput";
            Field<ControllerElementEnum>("layoutElement",
                resolve: context => context.Source.LayoutElement);
            Field<ControllerElementEnum>("deviceCapability",
                resolve: context => context.Source.DeviceCapability);
        }
    }
}
