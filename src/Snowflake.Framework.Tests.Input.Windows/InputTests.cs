using System;
using Xunit;
using SharpDX.XInput;
using SharpDX.DirectInput;
using System.Linq;
using System.Collections.Generic;
using System.Management;
using Snowflake.Support.InputEnumerators.Windows;
using System.Diagnostics;
using Reloaded.Injector;

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

        [Fact]
        public void InjectRetroArch()
        {
            var retroArchProcess = Process.Start("E:\\Emulators\\RetroArch\\retroarch.exe");
            var injector = new Injector(retroArchProcess);
            Debugger.Break();
            injector.Inject(@"D:\coding\snowflake\src\Snowflake.Support.Orchestration.Overlay.Runtime.Windows\bin\Debug\net6.0\kaku-x64.dll");
        }
    }
}
