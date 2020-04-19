using HotChocolate.Types;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerElementTypeEnum
        : EnumType<ControllerElementType>
    {
        protected override void Configure(IEnumTypeDescriptor<ControllerElementType> descriptor)
        {
            descriptor.Name("ControllerElementType")
                .Description("Stone-defined semantic types for Controller Elements." +
                    "Specifies the semantics of a specific element on a controller layout.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
