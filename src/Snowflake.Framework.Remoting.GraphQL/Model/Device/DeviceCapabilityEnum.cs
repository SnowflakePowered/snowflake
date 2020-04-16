using HotChocolate.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Device
{
    public sealed class DeviceCapabilityEnum
        : EnumType<DeviceCapability>
    {
        protected override void Configure(IEnumTypeDescriptor<DeviceCapability> descriptor)
        {
            descriptor.Name("DeviceCapability")
                .Description("Input device capabilities exposed by the underlying device API.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
