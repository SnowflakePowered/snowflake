using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQL.Types.InputDevice;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.InputDevice
{
    public class InputDeviceInputType : InputObjectGraphType<InputDeviceInputObject>
    {
        public InputDeviceInputType()
        {
            Name = "InputDeviceInput";
            Field<IntGraphType>("deviceIndex",
                resolve: context => context.Source.DeviceIndex,
                description: "The index of the device.");
            Field<NonNullGraphType<StringGraphType>>("deviceId",
                resolve: context => context.Source.DeviceId,
                description: "The ID of the device.");
            Field<InputApiEnum>("deviceApi",
                resolve: context => context.Source.DeviceApi,
                description: "The input API of the device.");
        }
    }
}
