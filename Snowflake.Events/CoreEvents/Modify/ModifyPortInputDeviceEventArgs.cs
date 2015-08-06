using Snowflake.Platform;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyPortInputDeviceEventArgs : SnowflakeEventArgs
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
                this.portNumber = value;
            }
        }
        int portNumber;
        public IPlatformInfo Platform { get; set; }
        public string PreviousPortInputDevice { get; private set; }
        public string ModifiedPortInputDevice { get; set; }
        public ModifyPortInputDeviceEventArgs(ICoreService eventCoreInstance, int portNumber, IPlatformInfo platformInfo, string previousPortInputDevice, string modifiedPortInputDevice)
            : base(eventCoreInstance)
        {
            this.portNumber = portNumber;
            this.Platform = platformInfo;
            this.PreviousPortInputDevice = previousPortInputDevice;
            this.ModifiedPortInputDevice = modifiedPortInputDevice;
        }
    }
}

