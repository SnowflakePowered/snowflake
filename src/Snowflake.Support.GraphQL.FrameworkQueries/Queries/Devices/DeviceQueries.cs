using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices
{
    public class DeviceQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.ExtendQuery();
            descriptor.Field("devices")
                .Type<NonNullType<ListType<NonNullType<InputDeviceType>>>>()
                .Description("Provides access to input devices on the system.")
                .Resolver(context =>
                {
                    var input = context.SnowflakeService<IDeviceEnumerator>();
                    return input.QueryConnectedDevices();
                });
        }
    }
}
