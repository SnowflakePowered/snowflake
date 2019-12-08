using System;
using Xunit;
using SharpDX.XInput;
using SharpDX.DirectInput;
using SharpDX.RawInput;
using System.Linq;
using System.Collections.Generic;
using System.Management;

namespace Snowflake.Framework.Tests.Input.Windows
{
    // var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

    public class InputTests
    {
        [Fact]
        public void Test1()
        {
            var xinputInstance = new Controller(UserIndex.One);
            var capabilities = xinputInstance.GetCapabilities(DeviceQueryType.Any);
            var state = xinputInstance.GetState();

            var directInput = new DirectInput();

            var devices = directInput.GetDevices(DeviceClass.GameControl,
                DeviceEnumerationFlags.AttachedOnly).Select(j => new Joystick(directInput, j.InstanceGuid))
                .ToList();
            devices[1].Acquire();
            var dstate = devices[1].GetCurrentState();

            var prop = devices[1].GetObjects();
        }
    }
}
