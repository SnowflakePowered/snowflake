using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class InputMapperTests
    {
        [Fact]
        public void GamepadMapping_Test()
        {
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            Assert.Equal(InputApi.XInput, mapping.InputApi);
            Assert.Equal("0", mapping[ControllerElement.ButtonA]);
            Assert.Equal("1", mapping[ControllerElement.ButtonB]);
            Assert.Equal("2", mapping[ControllerElement.ButtonX]);
            Assert.Equal("3", mapping[ControllerElement.ButtonY]);
            Assert.Equal("6", mapping[ControllerElement.ButtonStart]);
            Assert.Equal("7", mapping[ControllerElement.ButtonSelect]);
            Assert.Equal("4", mapping[ControllerElement.ButtonL]);
            Assert.Equal("5", mapping[ControllerElement.ButtonR]);
            Assert.Equal("8", mapping[ControllerElement.ButtonClickL]);
            Assert.Equal("9", mapping[ControllerElement.ButtonClickR]);
            Assert.Equal("10", mapping[ControllerElement.ButtonGuide]);
            Assert.Equal("h0up", mapping[ControllerElement.DirectionalN]);
            Assert.Equal("h0left", mapping[ControllerElement.DirectionalE]);
            Assert.Equal("h0down", mapping[ControllerElement.DirectionalS]);
            Assert.Equal("h0right", mapping[ControllerElement.DirectionalW]);
            Assert.Equal("+4", mapping[ControllerElement.TriggerLeft]);
            Assert.Equal("+5", mapping[ControllerElement.TriggerRight]);
            Assert.Equal("+0", mapping[ControllerElement.AxisLeftAnalogPositiveX]);
            Assert.Equal("-0", mapping[ControllerElement.AxisLeftAnalogNegativeX]);
            Assert.Equal("-1", mapping[ControllerElement.AxisLeftAnalogPositiveY]);
            Assert.Equal("+1", mapping[ControllerElement.AxisLeftAnalogNegativeY]);
            Assert.Equal("+2", mapping[ControllerElement.AxisRightAnalogPositiveX]);
            Assert.Equal("-2", mapping[ControllerElement.AxisRightAnalogNegativeX]);
            Assert.Equal("-3", mapping[ControllerElement.AxisRightAnalogPositiveY]);
            Assert.Equal("+3", mapping[ControllerElement.AxisRightAnalogNegativeY]);
            Assert.Equal(string.Empty, mapping[ControllerElement.RumbleBig]);
            Assert.Equal(string.Empty, mapping[ControllerElement.RumbleSmall]);
        }

        [Fact]
        public void KeyboardMapping_Test()
        {
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.KEYBOARD_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            Assert.Equal(InputApi.DirectInput, mapping.InputApi);
            Assert.Equal("nul", mapping[ControllerElement.KeyNone]);
            Assert.Equal("a", mapping[ControllerElement.KeyA]);
            Assert.Equal("b", mapping[ControllerElement.KeyB]);
            Assert.Equal("c", mapping[ControllerElement.KeyC]);
            Assert.Equal("d", mapping[ControllerElement.KeyD]);
            Assert.Equal("e", mapping[ControllerElement.KeyE]);
            Assert.Equal("f", mapping[ControllerElement.KeyF]);
            Assert.Equal("g", mapping[ControllerElement.KeyG]);
            Assert.Equal("h", mapping[ControllerElement.KeyH]);
            Assert.Equal("i", mapping[ControllerElement.KeyI]);
            Assert.Equal("j", mapping[ControllerElement.KeyJ]);
            Assert.Equal("k", mapping[ControllerElement.KeyK]);
            Assert.Equal("l", mapping[ControllerElement.KeyL]);
            Assert.Equal("m", mapping[ControllerElement.KeyM]);
            Assert.Equal("n", mapping[ControllerElement.KeyN]);
            Assert.Equal("o", mapping[ControllerElement.KeyO]);
            Assert.Equal("p", mapping[ControllerElement.KeyP]);
            Assert.Equal("q", mapping[ControllerElement.KeyQ]);
            Assert.Equal("r", mapping[ControllerElement.KeyR]);
            Assert.Equal("s", mapping[ControllerElement.KeyS]);
            Assert.Equal("t", mapping[ControllerElement.KeyT]);
            Assert.Equal("u", mapping[ControllerElement.KeyU]);
            Assert.Equal("v", mapping[ControllerElement.KeyV]);
            Assert.Equal("w", mapping[ControllerElement.KeyW]);
            Assert.Equal("x", mapping[ControllerElement.KeyX]);
            Assert.Equal("y", mapping[ControllerElement.KeyY]);
            Assert.Equal("z", mapping[ControllerElement.KeyZ]);
            Assert.Equal("0", mapping[ControllerElement.Key0]);
            Assert.Equal("1", mapping[ControllerElement.Key1]);
            Assert.Equal("2", mapping[ControllerElement.Key2]);
            Assert.Equal("3", mapping[ControllerElement.Key3]);
            Assert.Equal("4", mapping[ControllerElement.Key4]);
            Assert.Equal("5", mapping[ControllerElement.Key5]);
            Assert.Equal("6", mapping[ControllerElement.Key6]);
            Assert.Equal("7", mapping[ControllerElement.Key7]);
            Assert.Equal("8", mapping[ControllerElement.Key8]);
            Assert.Equal("9", mapping[ControllerElement.Key9]);
            Assert.Equal("equals", mapping[ControllerElement.KeyEquals]);
            Assert.Equal("minus", mapping[ControllerElement.KeyMinus]);
            Assert.Equal("backspace", mapping[ControllerElement.KeyBackspace]);
            Assert.Equal("space", mapping[ControllerElement.KeySpacebar]);
            Assert.Equal("enter", mapping[ControllerElement.KeyEnter]);
            Assert.Equal("keypad8", mapping[ControllerElement.KeyUp]);
            Assert.Equal("keypad2", mapping[ControllerElement.KeyDown]);
            Assert.Equal("keypad4", mapping[ControllerElement.KeyLeft]);
            Assert.Equal("keypad6", mapping[ControllerElement.KeyRight]);
            Assert.Equal("tab", mapping[ControllerElement.KeyTab]);
            Assert.Equal("insert", mapping[ControllerElement.KeyInsert]);
            Assert.Equal("delete", mapping[ControllerElement.KeyDelete]);
            Assert.Equal("home", mapping[ControllerElement.KeyHome]);
            Assert.Equal("end", mapping[ControllerElement.KeyEnd]);
            Assert.Equal("keypad9", mapping[ControllerElement.KeyPageUp]);
            Assert.Equal("keypad3", mapping[ControllerElement.KeyPageDown]);
            Assert.Equal("shift", mapping[ControllerElement.KeyShift]);
            Assert.Equal("ctrl", mapping[ControllerElement.KeyCtrl]);
            Assert.Equal("alt", mapping[ControllerElement.KeyAlt]);
            Assert.Equal("escape", mapping[ControllerElement.KeyEscape]);
            Assert.Equal("tilde", mapping[ControllerElement.KeyTilde]);
            Assert.Equal("quote", mapping[ControllerElement.KeyQuote]);
            Assert.Equal("semicolon", mapping[ControllerElement.KeySemicolon]);
            Assert.Equal("comma", mapping[ControllerElement.KeyComma]);
            Assert.Equal("period", mapping[ControllerElement.KeyPeriod]);
            Assert.Equal("slash", mapping[ControllerElement.KeySlash]);
            Assert.Equal("leftbracket", mapping[ControllerElement.KeyBracketLeft]);
            Assert.Equal("rightbracket", mapping[ControllerElement.KeyBracketRight]);
            Assert.Equal("backslash", mapping[ControllerElement.KeyBackslash]);
            Assert.Equal("alt", mapping[ControllerElement.KeyRightAlt]);
            Assert.Equal("ctrl", mapping[ControllerElement.KeyRightCtrl]);
            Assert.Equal("shift", mapping[ControllerElement.KeyRightShift]);
            Assert.Equal("num0", mapping[ControllerElement.KeyNum0]);
            Assert.Equal("num1", mapping[ControllerElement.KeyNum1]);
            Assert.Equal("num2", mapping[ControllerElement.KeyNum2]);
            Assert.Equal("num3", mapping[ControllerElement.KeyNum3]);
            Assert.Equal("num4", mapping[ControllerElement.KeyNum4]);
            Assert.Equal("num5", mapping[ControllerElement.KeyNum5]);
            Assert.Equal("num6", mapping[ControllerElement.KeyNum6]);
            Assert.Equal("num7", mapping[ControllerElement.KeyNum7]);
            Assert.Equal("num8", mapping[ControllerElement.KeyNum8]);
            Assert.Equal("num9", mapping[ControllerElement.KeyNum9]);
            Assert.Equal("kp_period", mapping[ControllerElement.KeyNumPeriod]);
            Assert.Equal("add", mapping[ControllerElement.KeyNumPlus]);
            Assert.Equal("subtract", mapping[ControllerElement.KeyNumMinus]);
            Assert.Equal("enter", mapping[ControllerElement.KeyNumEnter]);
            Assert.Equal("f1", mapping[ControllerElement.KeyF1]);
            Assert.Equal("f2", mapping[ControllerElement.KeyF2]);
            Assert.Equal("f3", mapping[ControllerElement.KeyF3]);
            Assert.Equal("f4", mapping[ControllerElement.KeyF4]);
            Assert.Equal("f5", mapping[ControllerElement.KeyF5]);
            Assert.Equal("f6", mapping[ControllerElement.KeyF6]);
            Assert.Equal("f7", mapping[ControllerElement.KeyF7]);
            Assert.Equal("f8", mapping[ControllerElement.KeyF8]);
            Assert.Equal("f9", mapping[ControllerElement.KeyF9]);
            Assert.Equal("f10", mapping[ControllerElement.KeyF10]);
            Assert.Equal("f11", mapping[ControllerElement.KeyF11]);
            Assert.Equal("f12", mapping[ControllerElement.KeyF12]);
        }

        [Fact]
        public void DefaultMappedElementCollection_Test()
        {
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            foreach (var controllerElem in from elem in mapcol
                where elem.DeviceElement != ControllerElement.NoElement
                select elem.DeviceElement)
            {
                Assert.NotNull(realmapping.Layout[controllerElem]);
            }
        }

        [Fact]
        public void InputTemplateGetterSetter_Test()
        {
            var realLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));

            var targetLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));

            var mapcol = ControllerElementMappings.GetDefaultMappings(realLayout, targetLayout);
            IInputTemplate template = new InputTemplate<IRetroArchInput>(mapcol, 0);

            Assert.Equal(ControllerElement.KeyZ, template[ControllerElement.ButtonA]);
            template[ControllerElement.ButtonA] = ControllerElement.KeyX;
            Assert.Equal(ControllerElement.KeyX, template[ControllerElement.ButtonA]);
        }
    }
}
