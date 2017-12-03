using GraphQL.Types;
using Snowflake.Input.Device;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice
{
    public class InputDeviceGraphType : ObjectGraphType<IInputDevice>
    {
        public InputDeviceGraphType()
        {
            Field<InputApiEnum>("inputApi",
                description: "Input API",
                resolve: context => context.Source.DeviceApi);
            Field<ControllerLayoutType>("deviceLayout",
               description: "Controller Layout",
               resolve: context => context.Source.DeviceLayout);
        }
    }
}
