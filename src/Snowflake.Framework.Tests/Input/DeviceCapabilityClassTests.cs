using EnumsNET;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace Snowflake.Input.Tests
{
    public class DeviceCapabilityClassTests
    {
        [Fact]
        public void KeyboardClass_Tests()
        {
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Keyboard, DeviceCapability.KeyA.GetClass()));
            Assert.False(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Controller, DeviceCapability.KeyA.GetClass()));

            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.KeyboardMouse, DeviceCapability.KeyA.GetClass()));
            Assert.False(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Mouse, DeviceCapability.KeyA.GetClass()));
        }

        [Fact]
        public void MouseClass_Tests()
        {
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Mouse, DeviceCapability.Mouse0.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.MouseButton, DeviceCapability.Mouse0.GetClass()));
            Assert.False(FlagEnums.HasAnyFlags(DeviceCapabilityClass.MouseCursor, DeviceCapability.Mouse0.GetClass()));

            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Mouse, DeviceCapability.CursorX.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.MouseCursor, DeviceCapability.CursorX.GetClass()));
            Assert.False(FlagEnums.HasAnyFlags(DeviceCapabilityClass.MouseButton, DeviceCapability.CursorX.GetClass()));
        }

        [Fact]
        public void ControllerClass_Tests()
        {
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Controller, DeviceCapability.Button0.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Controller, DeviceCapability.Axis0.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.Controller, DeviceCapability.Hat0E.GetClass()));

            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.ControllerDirectional, DeviceCapability.Hat0E.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.ControllerFaceButton, DeviceCapability.Button0.GetClass()));
            Assert.True(FlagEnums.HasAnyFlags(DeviceCapabilityClass.ControllerAxes, DeviceCapability.Axis0.GetClass()));
        }
    }
}
