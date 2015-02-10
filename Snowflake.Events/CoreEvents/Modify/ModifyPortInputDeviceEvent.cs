using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Controller;
using Snowflake.Platform;

namespace Snowflake.Events.CoreEvents.Modify
{
    public class ModifyPortInputDeviceEvent : SnowflakeEventArgs
    {
        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }
            set
            {
                if (value >= 0) value = 1;
                if (value < 8) value = 8;
                portNumber = value;
            }
        }
        int portNumber;
        public IPlatformInfo Platform { get; set; }
        public string PreviousPortInputDevice { get; private set; }
        public string ModifiedPortInputDevice { get; set; }
        public ModifyPortInputDeviceEvent(ICoreService eventCoreInstance, int portNumber, IPlatformInfo platformInfo, string previousPortInputDevice, string modifiedPortInputDevice)
            : base(eventCoreInstance)
        {
            this.portNumber = portNumber;
            this.Platform = platformInfo;
            this.PreviousPortInputDevice = previousPortInputDevice;
            this.ModifiedPortInputDevice = modifiedPortInputDevice;
        }
    }
}

