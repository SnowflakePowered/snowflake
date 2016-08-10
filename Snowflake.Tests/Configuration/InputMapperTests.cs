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
using Snowflake.Service;
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
            Assert.Equal(mapping.InputApi, InputApi.XInput);
            Assert.Equal(mapping[ControllerElement.ButtonA], "0");
            Assert.Equal(mapping[ControllerElement.ButtonB], "1");
            Assert.Equal(mapping[ControllerElement.ButtonX], "2");
            Assert.Equal(mapping[ControllerElement.ButtonY], "3");
            Assert.Equal(mapping[ControllerElement.ButtonStart], "6");
            Assert.Equal(mapping[ControllerElement.ButtonSelect], "7");
            Assert.Equal(mapping[ControllerElement.ButtonL], "4");
            Assert.Equal(mapping[ControllerElement.ButtonR], "5");
            Assert.Equal(mapping[ControllerElement.ButtonClickL], "8");
            Assert.Equal(mapping[ControllerElement.ButtonClickR], "9");
            Assert.Equal(mapping[ControllerElement.ButtonGuide], "10");
            Assert.Equal(mapping[ControllerElement.DirectionalN], "h0up");
            Assert.Equal(mapping[ControllerElement.DirectionalE], "h0left");
            Assert.Equal(mapping[ControllerElement.DirectionalS], "h0down");
            Assert.Equal(mapping[ControllerElement.DirectionalW], "h0right");
            Assert.Equal(mapping[ControllerElement.TriggerLeft], "+4");
            Assert.Equal(mapping[ControllerElement.TriggerRight], "+5");
            Assert.Equal(mapping[ControllerElement.AxisLeftAnalogPositiveX], "+0");
            Assert.Equal(mapping[ControllerElement.AxisLeftAnalogNegativeX], "-0");
            Assert.Equal(mapping[ControllerElement.AxisLeftAnalogPositiveY], "-1");
            Assert.Equal(mapping[ControllerElement.AxisLeftAnalogNegativeY], "+1");
            Assert.Equal(mapping[ControllerElement.AxisRightAnalogPositiveX], "+2");
            Assert.Equal(mapping[ControllerElement.AxisRightAnalogNegativeX], "-2");
            Assert.Equal(mapping[ControllerElement.AxisRightAnalogPositiveY], "-3");
            Assert.Equal(mapping[ControllerElement.AxisRightAnalogNegativeY], "+3");
            Assert.Equal(mapping[ControllerElement.RumbleBig], "");
            Assert.Equal(mapping[ControllerElement.RumbleSmall], "");
        }

        [Fact]
        public void KeyboardMapping_Test()
        {
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.KEYBOARD_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            Assert.Equal(mapping.InputApi, InputApi.DirectInput);
            Assert.Equal(mapping[KeyboardKey.KeyNone], "nul");
            Assert.Equal(mapping[KeyboardKey.KeyA], "a");
            Assert.Equal(mapping[KeyboardKey.KeyB], "b");
            Assert.Equal(mapping[KeyboardKey.KeyC], "c");
            Assert.Equal(mapping[KeyboardKey.KeyD], "d");
            Assert.Equal(mapping[KeyboardKey.KeyE], "e");
            Assert.Equal(mapping[KeyboardKey.KeyF], "f");
            Assert.Equal(mapping[KeyboardKey.KeyG], "g");
            Assert.Equal(mapping[KeyboardKey.KeyH], "h");
            Assert.Equal(mapping[KeyboardKey.KeyI], "i");
            Assert.Equal(mapping[KeyboardKey.KeyJ], "j");
            Assert.Equal(mapping[KeyboardKey.KeyK], "k");
            Assert.Equal(mapping[KeyboardKey.KeyL], "l");
            Assert.Equal(mapping[KeyboardKey.KeyM], "m");
            Assert.Equal(mapping[KeyboardKey.KeyN], "n");
            Assert.Equal(mapping[KeyboardKey.KeyO], "o");
            Assert.Equal(mapping[KeyboardKey.KeyP], "p");
            Assert.Equal(mapping[KeyboardKey.KeyQ], "q");
            Assert.Equal(mapping[KeyboardKey.KeyR], "r");
            Assert.Equal(mapping[KeyboardKey.KeyS], "s");
            Assert.Equal(mapping[KeyboardKey.KeyT], "t");
            Assert.Equal(mapping[KeyboardKey.KeyU], "u");
            Assert.Equal(mapping[KeyboardKey.KeyV], "v");
            Assert.Equal(mapping[KeyboardKey.KeyW], "w");
            Assert.Equal(mapping[KeyboardKey.KeyX], "x");
            Assert.Equal(mapping[KeyboardKey.KeyY], "y");
            Assert.Equal(mapping[KeyboardKey.KeyZ], "z");
            Assert.Equal(mapping[KeyboardKey.Key0], "0");
            Assert.Equal(mapping[KeyboardKey.Key1], "1");
            Assert.Equal(mapping[KeyboardKey.Key2], "2");
            Assert.Equal(mapping[KeyboardKey.Key3], "3");
            Assert.Equal(mapping[KeyboardKey.Key4], "4");
            Assert.Equal(mapping[KeyboardKey.Key5], "5");
            Assert.Equal(mapping[KeyboardKey.Key6], "6");
            Assert.Equal(mapping[KeyboardKey.Key7], "7");
            Assert.Equal(mapping[KeyboardKey.Key8], "8");
            Assert.Equal(mapping[KeyboardKey.Key9], "9");
            Assert.Equal(mapping[KeyboardKey.KeyEquals], "equals");
            Assert.Equal(mapping[KeyboardKey.KeyMinus], "minus");
            Assert.Equal(mapping[KeyboardKey.KeyBackspace], "backspace");
            Assert.Equal(mapping[KeyboardKey.KeySpacebar], "space");
            Assert.Equal(mapping[KeyboardKey.KeyEnter], "enter");
            Assert.Equal(mapping[KeyboardKey.KeyUp], "keypad8");
            Assert.Equal(mapping[KeyboardKey.KeyDown], "keypad2");
            Assert.Equal(mapping[KeyboardKey.KeyLeft], "keypad4");
            Assert.Equal(mapping[KeyboardKey.KeyRight], "keypad6");
            Assert.Equal(mapping[KeyboardKey.KeyTab], "tab");
            Assert.Equal(mapping[KeyboardKey.KeyInsert], "insert");
            Assert.Equal(mapping[KeyboardKey.KeyDelete], "delete");
            Assert.Equal(mapping[KeyboardKey.KeyHome], "home");
            Assert.Equal(mapping[KeyboardKey.KeyEnd], "end");
            Assert.Equal(mapping[KeyboardKey.KeyPageUp], "keypad9");
            Assert.Equal(mapping[KeyboardKey.KeyPageDown], "keypad3");
            Assert.Equal(mapping[KeyboardKey.KeyShift], "shift");
            Assert.Equal(mapping[KeyboardKey.KeyCtrl], "ctrl");
            Assert.Equal(mapping[KeyboardKey.KeyAlt], "alt");
            Assert.Equal(mapping[KeyboardKey.KeyEscape], "escape");
            Assert.Equal(mapping[KeyboardKey.KeyTilde], "tilde");
            Assert.Equal(mapping[KeyboardKey.KeyQuote], "quote");
            Assert.Equal(mapping[KeyboardKey.KeySemicolon], "semicolon");
            Assert.Equal(mapping[KeyboardKey.KeyComma], "comma");
            Assert.Equal(mapping[KeyboardKey.KeyPeriod], "period");
            Assert.Equal(mapping[KeyboardKey.KeySlash], "slash");
            Assert.Equal(mapping[KeyboardKey.KeyBracketLeft], "leftbracket");
            Assert.Equal(mapping[KeyboardKey.KeyBracketRight], "rightbracket");
            Assert.Equal(mapping[KeyboardKey.KeyBackslash], "backslash");
            Assert.Equal(mapping[KeyboardKey.KeyRightAlt], "alt");
            Assert.Equal(mapping[KeyboardKey.KeyRightCtrl], "ctrl");
            Assert.Equal(mapping[KeyboardKey.KeyRightShift], "shift");
            Assert.Equal(mapping[KeyboardKey.KeyNum0], "num0");
            Assert.Equal(mapping[KeyboardKey.KeyNum1], "num1");
            Assert.Equal(mapping[KeyboardKey.KeyNum2], "num2");
            Assert.Equal(mapping[KeyboardKey.KeyNum3], "num3");
            Assert.Equal(mapping[KeyboardKey.KeyNum4], "num4");
            Assert.Equal(mapping[KeyboardKey.KeyNum5], "num5");
            Assert.Equal(mapping[KeyboardKey.KeyNum6], "num6");
            Assert.Equal(mapping[KeyboardKey.KeyNum7], "num7");
            Assert.Equal(mapping[KeyboardKey.KeyNum8], "num8");
            Assert.Equal(mapping[KeyboardKey.KeyNum9], "num9");
            Assert.Equal(mapping[KeyboardKey.KeyNumPeriod], "kp_period");
            Assert.Equal(mapping[KeyboardKey.KeyNumPlus], "add");
            Assert.Equal(mapping[KeyboardKey.KeyNumMinus], "subtract");
            Assert.Equal(mapping[KeyboardKey.KeyNumEnter], "enter");
            Assert.Equal(mapping[KeyboardKey.KeyF1], "f1");
            Assert.Equal(mapping[KeyboardKey.KeyF2], "f2");
            Assert.Equal(mapping[KeyboardKey.KeyF3], "f3");
            Assert.Equal(mapping[KeyboardKey.KeyF4], "f4");
            Assert.Equal(mapping[KeyboardKey.KeyF5], "f5");
            Assert.Equal(mapping[KeyboardKey.KeyF6], "f6");
            Assert.Equal(mapping[KeyboardKey.KeyF7], "f7");
            Assert.Equal(mapping[KeyboardKey.KeyF8], "f8");
            Assert.Equal(mapping[KeyboardKey.KeyF9], "f9");
            Assert.Equal(mapping[KeyboardKey.KeyF10], "f10");
            Assert.Equal(mapping[KeyboardKey.KeyF11], "f11");
            Assert.Equal(mapping[KeyboardKey.KeyF12], "f12");
        }

        [Fact]
        public void DefaultMappedElementCollection_Test()
        {
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            foreach (var controllerElem in (from elem in mapcol where elem.DeviceElement != ControllerElement.NoElement
                                            select elem.DeviceElement))
            {
                Assert.NotNull(realmapping.Layout[controllerElem]);
            }
        }
        [Fact]
        public void KvpSerialize_Test()
        {
            var serializer = new KeyValuePairConfigurationSerializer(new BooleanMapping("true", "false"), "nul", "=");
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            var template = new TestInputTemplate();
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var device = new Moq.Mock<IInputDevice>();
            device.SetupGet(x => x.DeviceApi).Returns(InputApi.XInput);
            device.SetupGet(x => x.DeviceLayout).Returns(realmapping);
            template.SetInputValues(mapcol, device.Object, 0);
            string serializedValue = serializer.Serialize(template, mapping).Replace(Environment.NewLine, "");
            Assert.Equal(
                TestUtilities.GetStringResource("Configurations.ExampleInput.cfg").Replace(Environment.NewLine, ""),
                serializedValue);
        }
        [Fact]
        public void InitSerialize_Test()
        {
            var serializer = new IniConfigurationSerializer(new BooleanMapping("true", "false"), "nul");
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            var template = new TestInputTemplate();
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var device = new Moq.Mock<IInputDevice>();
            device.SetupGet(x => x.DeviceApi).Returns(InputApi.XInput);
            device.SetupGet(x => x.DeviceLayout).Returns(realmapping);
            template.SetInputValues(mapcol, device.Object, 0);
            string serializedValue = serializer.Serialize(template, mapping).Replace(Environment.NewLine, "");
            Assert.Equal(
                TestUtilities.GetStringResource("Configurations.ExampleInput.ini").Replace(Environment.NewLine, ""),
                serializedValue);
        }
    }
}
