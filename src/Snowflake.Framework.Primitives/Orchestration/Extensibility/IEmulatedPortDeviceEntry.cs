using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    public interface IEmulatedPortDeviceEntry
    {
        InputDriverType Driver { get; }
        string DeviceName { get; }
        int UniqueNameEnumerationIndex { get; }
        ControllerId ControllerID { get; }
        PlatformId PlatformID { get; }
        string ProfileName { get; }
        int PortIndex { get; }
    }
}
