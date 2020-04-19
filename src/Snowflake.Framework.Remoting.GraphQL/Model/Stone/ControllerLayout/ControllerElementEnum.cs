using HotChocolate.Types;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerElementEnum
        : EnumType<ControllerElement>
    {
        protected override void Configure(IEnumTypeDescriptor<ControllerElement> descriptor)
        {
            descriptor.Name("ControllerElement")
                .Description("Stone-defined semantic Controller Elements. " +
                    "A Controller Element is a semantic name given to a capability of a defined Stone controller layout.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
