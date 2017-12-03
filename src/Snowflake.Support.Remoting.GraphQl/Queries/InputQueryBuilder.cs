using Snowflake.Configuration;
using Snowflake.Input.Device;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;
using Snowflake.Support.Remoting.GraphQl.Types.InputDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class InputQueryBuilder : QueryBuilder
    {
        public IInputManager Manager { get; }
        public IPluginManager Plugins { get; }
        public IEnumerable<IInputEnumerator> Enumerators => this.Plugins.Get<IInputEnumerator>();
        public InputQueryBuilder(IInputManager manager, IPluginManager pluginManager)
        {
            this.Manager = manager;
            this.Plugins = pluginManager;
        }

        [Connection("lowLevelInputDevices", "Gets all enumerated input devices on this computer.", typeof(LowLevelInputDeviceGraphType))]
        public IEnumerable<ILowLevelInputDevice> GetLLInputs()
        {
            return this.Manager.GetAllDevices();
        }

        [Connection("inputDevices", "Gets the sorted input devices on this computer.", typeof(InputDeviceGraphType))]
        public IEnumerable<IInputDevice> GetAllInputDevices()
        {
            return this.Enumerators.SelectMany(p => p.GetConnectedDevices());
        }

    }
}
