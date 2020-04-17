using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Device;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class InputDriverEnum : EnumerationGraphType<InputDriver>
    {
        public InputDriverEnum()
        {
            Name = "InputDriver";
            Description = "Possible drivers for input device instances";
        }
    }
}
