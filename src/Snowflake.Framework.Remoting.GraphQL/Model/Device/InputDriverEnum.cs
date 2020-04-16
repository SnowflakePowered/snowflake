using HotChocolate.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Device
{
    public sealed class InputDriverEnum
        : EnumType<InputDriver>
    {
        protected override void Configure(IEnumTypeDescriptor<InputDriver> descriptor)
        {
            descriptor.Name("InputDriver")
                .Description("Input device capabilities exposed by the underlying device API.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
