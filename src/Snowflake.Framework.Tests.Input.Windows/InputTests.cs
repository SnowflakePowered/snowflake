using System;
using Xunit;
using SharpDX.XInput;
using SharpDX.DirectInput;
using System.Linq;
using System.Collections.Generic;
using System.Management;
using Snowflake.Support.InputEnumerators.Windows;

namespace Snowflake.Input.Tests.Windows
{
    // var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

    public class InputTests
    {
        [Fact]
        public void Test1()
        {
            var e = new WindowsDeviceEnumerator();
            var devices = e.QueryConnectedDevices().ToList();
        }
    }
}
