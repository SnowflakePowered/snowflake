﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement
{
    public class MappedControllerElementInputType : InputObjectGraphType<MappedControllerElementInputObject>
    {
        public MappedControllerElementInputType()
        {
            Field<ControllerElementEnum>("layoutElement",
                resolve: context => context.Source.LayoutElement);
            Field<ControllerElementEnum>("deviceElement",
                resolve: context => context.Source.DeviceElement);
        }
    }
}
