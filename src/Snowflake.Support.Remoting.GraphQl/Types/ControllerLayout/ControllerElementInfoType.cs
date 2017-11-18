using GraphQL.Types;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout
{
    public class ControllerElementInfoType : ObjectGraphType<KeyValuePair<ControllerElement, IControllerElementInfo>>
    {
        public ControllerElementInfoType()
        {
            Name = "ControllerElementInfo";
            Description = "A layout element of a controller in a controller layout.";
            Field(e => e.Value.Label).Description("The label of this controller element.");
            Field<ControllerElementTypeEnum>("type", 
                description: "The element type of this controller element.",
                resolve: context => context.Source.Value.Type);
            Field<ControllerElementEnum>("element",
                description: "The semantic element this controller element represents.",
                resolve: context => context.Source.Key);
        }
    }
}
