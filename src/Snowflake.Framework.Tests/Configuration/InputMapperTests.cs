using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Input.Tests;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class InputMapperTests
    {
        //[Fact]
        //public void GamepadMapping_Test()
        //{
        //    string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
        //    IDeviceInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
        //    Assert.Equal("0", mapping[ControllerElement.ButtonA]);
        //    Assert.Equal("1", mapping[ControllerElement.ButtonB]);
        //    Assert.Equal("2", mapping[ControllerElement.ButtonX]);
        //    Assert.Equal("3", mapping[ControllerElement.ButtonY]);
        //    Assert.Equal("6", mapping[ControllerElement.ButtonStart]);
        //    Assert.Equal("7", mapping[ControllerElement.ButtonSelect]);
        //    Assert.Equal("4", mapping[ControllerElement.ButtonL]);
        //    Assert.Equal("5", mapping[ControllerElement.ButtonR]);
        //    Assert.Equal("8", mapping[ControllerElement.ButtonClickL]);
        //    Assert.Equal("9", mapping[ControllerElement.ButtonClickR]);
        //    Assert.Equal("10", mapping[ControllerElement.ButtonGuide]);
        //    Assert.Equal("h0up", mapping[ControllerElement.DirectionalN]);
        //    Assert.Equal("h0left", mapping[ControllerElement.DirectionalE]);
        //    Assert.Equal("h0down", mapping[ControllerElement.DirectionalS]);
        //    Assert.Equal("h0right", mapping[ControllerElement.DirectionalW]);
        //    Assert.Equal("+4", mapping[ControllerElement.TriggerLeft]);
        //    Assert.Equal("+5", mapping[ControllerElement.TriggerRight]);
        //    Assert.Equal("+0", mapping[ControllerElement.AxisLeftAnalogPositiveX]);
        //    Assert.Equal("-0", mapping[ControllerElement.AxisLeftAnalogNegativeX]);
        //    Assert.Equal("-1", mapping[ControllerElement.AxisLeftAnalogPositiveY]);
        //    Assert.Equal("+1", mapping[ControllerElement.AxisLeftAnalogNegativeY]);
        //    Assert.Equal("+2", mapping[ControllerElement.AxisRightAnalogPositiveX]);
        //    Assert.Equal("-2", mapping[ControllerElement.AxisRightAnalogNegativeX]);
        //    Assert.Equal("-3", mapping[ControllerElement.AxisRightAnalogPositiveY]);
        //    Assert.Equal("+3", mapping[ControllerElement.AxisRightAnalogNegativeY]);
        //    Assert.Equal(string.Empty, mapping[ControllerElement.RumbleBig]);
        //    Assert.Equal(string.Empty, mapping[ControllerElement.RumbleSmall]);
        //}

        [Fact]
        public void KeyboardMapping_Test()
        {
            string _mapping = TestUtilities.GetStringResource("InputMappings.inputmapping-retroarch.json");
            IDeviceInputMapping mapping = JsonSerializer.Deserialize<DictionaryInputMapping>(_mapping);
            Assert.Equal("a", mapping[DeviceCapability.KeyA]);
            Assert.Equal("b", mapping[DeviceCapability.KeyB]);
            Assert.Equal("c", mapping[DeviceCapability.KeyC]);
            Assert.Equal("d", mapping[DeviceCapability.KeyD]);
            Assert.Equal("e", mapping[DeviceCapability.KeyE]);
            Assert.Equal("f", mapping[DeviceCapability.KeyF]);
            Assert.Equal("g", mapping[DeviceCapability.KeyG]);
            Assert.Equal("h", mapping[DeviceCapability.KeyH]);
            Assert.Equal("i", mapping[DeviceCapability.KeyI]);
            Assert.Equal("j", mapping[DeviceCapability.KeyJ]);
            Assert.Equal("k", mapping[DeviceCapability.KeyK]);
            Assert.Equal("l", mapping[DeviceCapability.KeyL]);
            Assert.Equal("m", mapping[DeviceCapability.KeyM]);
            Assert.Equal("n", mapping[DeviceCapability.KeyN]);
            Assert.Equal("o", mapping[DeviceCapability.KeyO]);
            Assert.Equal("p", mapping[DeviceCapability.KeyP]);
            Assert.Equal("q", mapping[DeviceCapability.KeyQ]);
            Assert.Equal("r", mapping[DeviceCapability.KeyR]);
            Assert.Equal("s", mapping[DeviceCapability.KeyS]);
            Assert.Equal("t", mapping[DeviceCapability.KeyT]);
            Assert.Equal("u", mapping[DeviceCapability.KeyU]);
            Assert.Equal("v", mapping[DeviceCapability.KeyV]);
            Assert.Equal("w", mapping[DeviceCapability.KeyW]);
            Assert.Equal("x", mapping[DeviceCapability.KeyX]);
            Assert.Equal("y", mapping[DeviceCapability.KeyY]);
            Assert.Equal("z", mapping[DeviceCapability.KeyZ]);
            Assert.Equal("0", mapping[DeviceCapability.Key0]);
            Assert.Equal("1", mapping[DeviceCapability.Key1]);
            Assert.Equal("2", mapping[DeviceCapability.Key2]);
            Assert.Equal("3", mapping[DeviceCapability.Key3]);
            Assert.Equal("4", mapping[DeviceCapability.Key4]);
            Assert.Equal("5", mapping[DeviceCapability.Key5]);
            Assert.Equal("6", mapping[DeviceCapability.Key6]);
            Assert.Equal("7", mapping[DeviceCapability.Key7]);
            Assert.Equal("8", mapping[DeviceCapability.Key8]);
            Assert.Equal("9", mapping[DeviceCapability.Key9]);
            Assert.Equal("equals", mapping[DeviceCapability.KeyEquals]);
            Assert.Equal("minus", mapping[DeviceCapability.KeyMinus]);
            Assert.Equal("backspace", mapping[DeviceCapability.KeyBackspace]);
            Assert.Equal("space", mapping[DeviceCapability.KeySpacebar]);
            Assert.Equal("enter", mapping[DeviceCapability.KeyEnter]);
            Assert.Equal("keypad8", mapping[DeviceCapability.KeyUp]);
            Assert.Equal("keypad2", mapping[DeviceCapability.KeyDown]);
            Assert.Equal("keypad4", mapping[DeviceCapability.KeyLeft]);
            Assert.Equal("keypad6", mapping[DeviceCapability.KeyRight]);
            Assert.Equal("tab", mapping[DeviceCapability.KeyTab]);
            Assert.Equal("insert", mapping[DeviceCapability.KeyInsert]);
            Assert.Equal("delete", mapping[DeviceCapability.KeyDelete]);
            Assert.Equal("home", mapping[DeviceCapability.KeyHome]);
            Assert.Equal("end", mapping[DeviceCapability.KeyEnd]);
            Assert.Equal("keypad9", mapping[DeviceCapability.KeyPageUp]);
            Assert.Equal("keypad3", mapping[DeviceCapability.KeyPageDown]);
            Assert.Equal("shift", mapping[DeviceCapability.KeyShift]);
            Assert.Equal("ctrl", mapping[DeviceCapability.KeyCtrl]);
            Assert.Equal("alt", mapping[DeviceCapability.KeyAlt]);
            Assert.Equal("escape", mapping[DeviceCapability.KeyEscape]);
            Assert.Equal("tilde", mapping[DeviceCapability.KeyTilde]);
            Assert.Equal("quote", mapping[DeviceCapability.KeyQuote]);
            Assert.Equal("semicolon", mapping[DeviceCapability.KeySemicolon]);
            Assert.Equal("comma", mapping[DeviceCapability.KeyComma]);
            Assert.Equal("period", mapping[DeviceCapability.KeyPeriod]);
            Assert.Equal("slash", mapping[DeviceCapability.KeySlash]);
            Assert.Equal("leftbracket", mapping[DeviceCapability.KeyBracketLeft]);
            Assert.Equal("rightbracket", mapping[DeviceCapability.KeyBracketRight]);
            Assert.Equal("backslash", mapping[DeviceCapability.KeyBackslash]);
            Assert.Equal("alt", mapping[DeviceCapability.KeyRightAlt]);
            Assert.Equal("ctrl", mapping[DeviceCapability.KeyRightCtrl]);
            Assert.Equal("shift", mapping[DeviceCapability.KeyRightShift]);
            Assert.Equal("num0", mapping[DeviceCapability.KeyNum0]);
            Assert.Equal("num1", mapping[DeviceCapability.KeyNum1]);
            Assert.Equal("num2", mapping[DeviceCapability.KeyNum2]);
            Assert.Equal("num3", mapping[DeviceCapability.KeyNum3]);
            Assert.Equal("num4", mapping[DeviceCapability.KeyNum4]);
            Assert.Equal("num5", mapping[DeviceCapability.KeyNum5]);
            Assert.Equal("num6", mapping[DeviceCapability.KeyNum6]);
            Assert.Equal("num7", mapping[DeviceCapability.KeyNum7]);
            Assert.Equal("num8", mapping[DeviceCapability.KeyNum8]);
            Assert.Equal("num9", mapping[DeviceCapability.KeyNum9]);
            Assert.Equal("kp_period", mapping[DeviceCapability.KeyNumPeriod]);
            Assert.Equal("add", mapping[DeviceCapability.KeyNumPlus]);
            Assert.Equal("subtract", mapping[DeviceCapability.KeyNumMinus]);
            Assert.Equal("enter", mapping[DeviceCapability.KeyNumEnter]);
            Assert.Equal("f1", mapping[DeviceCapability.KeyF1]);
            Assert.Equal("f2", mapping[DeviceCapability.KeyF2]);
            Assert.Equal("f3", mapping[DeviceCapability.KeyF3]);
            Assert.Equal("f4", mapping[DeviceCapability.KeyF4]);
            Assert.Equal("f5", mapping[DeviceCapability.KeyF5]);
            Assert.Equal("f6", mapping[DeviceCapability.KeyF6]);
            Assert.Equal("f7", mapping[DeviceCapability.KeyF7]);
            Assert.Equal("f8", mapping[DeviceCapability.KeyF8]);
            Assert.Equal("f9", mapping[DeviceCapability.KeyF9]);
            Assert.Equal("f10", mapping[DeviceCapability.KeyF10]);
            Assert.Equal("f11", mapping[DeviceCapability.KeyF11]);
            Assert.Equal("f12", mapping[DeviceCapability.KeyF12]);
        }

        [Fact]
        public void InputTemplateGetterSetter_Test()
        {
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriver.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);

            var template = new InputConfiguration<IRetroArchInput>(mapcol, 0);

            Assert.Equal(DeviceCapability.Button0, template.Configuration.InputPlayerABtn);
            template[ControllerElement.ButtonA] = DeviceCapability.Button1;
            template[ControllerElement.ButtonA] = DeviceCapability.Key2;
            template[ControllerElement.ButtonA] = DeviceCapability.Axis0Negative;

            Assert.Equal(DeviceCapability.Button1, template.Configuration.InputPlayerABtn);
            Assert.Equal(DeviceCapability.Key2, template.Configuration.InputPlayerA);
            Assert.Equal(DeviceCapability.Axis0Negative, template.Configuration.InputPlayerAAxis);
        }
    }
}
