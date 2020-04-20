using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices
{
    public sealed class DeviceNodeQueries
        : ObjectTypeExtension<IInputDevice>
    {
        protected override void Configure(IObjectTypeDescriptor<IInputDevice> descriptor)
        {
            descriptor.Name("InputDevice");
            descriptor.Interface<NodeType>();
            descriptor.Field("id")
                .Type<IdType>()
                .Resolver(ctx => ctx.Parent<IInputDevice>().InstanceGuid);

            descriptor.AsNode()
                .NodeResolver<Guid>((ctx, id) => 
                    Task.FromResult(
                        ctx.SnowflakeService<IDeviceEnumerator>().QueryConnectedDevices().FirstOrDefault(i => i.InstanceGuid == id)));
        }
    }
}
