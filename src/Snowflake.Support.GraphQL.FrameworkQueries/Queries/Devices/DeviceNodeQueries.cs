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
            descriptor.Implements<NodeType>();
          
            descriptor
                .ImplementsNode()
                .IdField(t => t.InstanceGuid)
                .ResolveNode((ctx, id) => Task.FromResult(
                        ctx.SnowflakeService<IDeviceEnumerator>().QueryConnectedDevices().FirstOrDefault(i => i.InstanceGuid == id)));
        }
    }
}
