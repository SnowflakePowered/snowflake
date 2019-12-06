using System;
using Xunit;
using SharpDX.XInput;
using SharpDX.DirectInput;
using System.Linq;

namespace Snowflake.Framework.Tests.Input.Windows
{
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
                DeviceEnumerationFlags.AttachedOnly)
                .Select(d => new Joystick(directInput, d.InstanceGuid)).ToList(); 

        }
    }
}
