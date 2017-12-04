using GraphQL.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice
{
    public class InputApiEnum : EnumerationGraphType<InputApi>
    {
        public InputApiEnum()
        {
            Name = "InputApi";
            Description = "Input APIs supported by Snowflake.";
        }
    }
}
