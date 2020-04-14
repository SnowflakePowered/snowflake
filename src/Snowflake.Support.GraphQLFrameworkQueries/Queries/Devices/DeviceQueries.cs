using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Device;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices
{
    public class DeviceQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("devices")
                .Type<ListType<InputDeviceType>>()
                .Description("Provides access to input devices on the system.")
                .Resolver(context =>
                {
                    var input = context.Service<IDeviceEnumerator>();
                    return input.QueryConnectedDevices();
                });
        }
    }
}
