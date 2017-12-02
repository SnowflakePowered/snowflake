using Snowflake.Configuration;
using Snowflake.Input.Device;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;
using Snowflake.Support.Remoting.GraphQl.Types.InputDevice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class InputQueryBuilder : QueryBuilder
    {
        public IInputManager Manager { get; }
        public InputQueryBuilder(IInputManager manager)
        {
            this.Manager = manager;
        }

        [Connection("llInputList", "LLIpnuts", typeof(LowLevelInputDeviceGraphType))]
        public IEnumerable<ILowLevelInputDevice> GetLLInputs()
        {
            return this.Manager.GetAllDevices();
        }
    }
}
