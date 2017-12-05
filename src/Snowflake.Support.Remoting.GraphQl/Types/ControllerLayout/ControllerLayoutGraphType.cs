using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller;

namespace Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout
{
    public class ControllerLayoutGraphType : ObjectGraphType<IControllerLayout>
    {
        public ControllerLayoutGraphType()
        {
            Name = "ControllerLayout";
            Description = "A Stone Controller Layout";
            Field(c => c.FriendlyName).Description("The Friendly Name of this controller layout");
            Field(c => c.LayoutID).Description("The Stone Layout ID for this controller layout");
            Field<ListGraphType<StringGraphType>>(
                "platforms",
                description: "The platforms this controller supports.",
                resolve: context => context.Source.Platforms.ToList());
            Field<ListGraphType<ControllerElementInfoGraphType>>(
                "layout",
                description: "The layout of this controller.",
                resolve: context => context.Source.Layout.ToList());
        }
    }
}
