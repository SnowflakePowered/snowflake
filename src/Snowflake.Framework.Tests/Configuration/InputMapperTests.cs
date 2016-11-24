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
            Assert.Equal(mapping[ControllerElement.KeyNone], "nul");
            Assert.Equal(mapping[ControllerElement.KeyA], "a");
            Assert.Equal(mapping[ControllerElement.KeyB], "b");
            Assert.Equal(mapping[ControllerElement.KeyC], "c");
            Assert.Equal(mapping[ControllerElement.KeyD], "d");
            Assert.Equal(mapping[ControllerElement.KeyE], "e");
            Assert.Equal(mapping[ControllerElement.KeyF], "f");
            Assert.Equal(mapping[ControllerElement.KeyG], "g");
            Assert.Equal(mapping[ControllerElement.KeyH], "h");
            Assert.Equal(mapping[ControllerElement.KeyI], "i");
            Assert.Equal(mapping[ControllerElement.KeyJ], "j");
            Assert.Equal(mapping[ControllerElement.KeyK], "k");
            Assert.Equal(mapping[ControllerElement.KeyL], "l");
            Assert.Equal(mapping[ControllerElement.KeyM], "m");
            Assert.Equal(mapping[ControllerElement.KeyN], "n");
            Assert.Equal(mapping[ControllerElement.KeyO], "o");
            Assert.Equal(mapping[ControllerElement.KeyP], "p");
            Assert.Equal(mapping[ControllerElement.KeyQ], "q");
            Assert.Equal(mapping[ControllerElement.KeyR], "r");
            Assert.Equal(mapping[ControllerElement.KeyS], "s");
            Assert.Equal(mapping[ControllerElement.KeyT], "t");
            Assert.Equal(mapping[ControllerElement.KeyU], "u");
            Assert.Equal(mapping[ControllerElement.KeyV], "v");
            Assert.Equal(mapping[ControllerElement.KeyW], "w");
            Assert.Equal(mapping[ControllerElement.KeyX], "x");
            Assert.Equal(mapping[ControllerElement.KeyY], "y");
            Assert.Equal(mapping[ControllerElement.KeyZ], "z");
            Assert.Equal(mapping[ControllerElement.Key0], "0");
            Assert.Equal(mapping[ControllerElement.Key1], "1");
            Assert.Equal(mapping[ControllerElement.Key2], "2");
            Assert.Equal(mapping[ControllerElement.Key3], "3");
            Assert.Equal(mapping[ControllerElement.Key4], "4");
            Assert.Equal(mapping[ControllerElement.Key5], "5");
            Assert.Equal(mapping[ControllerElement.Key6], "6");
            Assert.Equal(mapping[ControllerElement.Key7], "7");
            Assert.Equal(mapping[ControllerElement.Key8], "8");
            Assert.Equal(mapping[ControllerElement.Key9], "9");
            Assert.Equal(mapping[ControllerElement.KeyEquals], "equals");
            Assert.Equal(mapping[ControllerElement.KeyMinus], "minus");
            Assert.Equal(mapping[ControllerElement.KeyBackspace], "backspace");
            Assert.Equal(mapping[ControllerElement.KeySpacebar], "space");
            Assert.Equal(mapping[ControllerElement.KeyEnter], "enter");
            Assert.Equal(mapping[ControllerElement.KeyUp], "keypad8");
            Assert.Equal(mapping[ControllerElement.KeyDown], "keypad2");
            Assert.Equal(mapping[ControllerElement.KeyLeft], "keypad4");
            Assert.Equal(mapping[ControllerElement.KeyRight], "keypad6");
            Assert.Equal(mapping[ControllerElement.KeyTab], "tab");
            Assert.Equal(mapping[ControllerElement.KeyInsert], "insert");
            Assert.Equal(mapping[ControllerElement.KeyDelete], "delete");
            Assert.Equal(mapping[ControllerElement.KeyHome], "home");
            Assert.Equal(mapping[ControllerElement.KeyEnd], "end");
            Assert.Equal(mapping[ControllerElement.KeyPageUp], "keypad9");
            Assert.Equal(mapping[ControllerElement.KeyPageDown], "keypad3");
            Assert.Equal(mapping[ControllerElement.KeyShift], "shift");
            Assert.Equal(mapping[ControllerElement.KeyCtrl], "ctrl");
            Assert.Equal(mapping[ControllerElement.KeyAlt], "alt");
            Assert.Equal(mapping[ControllerElement.KeyEscape], "escape");
            Assert.Equal(mapping[ControllerElement.KeyTilde], "tilde");
            Assert.Equal(mapping[ControllerElement.KeyQuote], "quote");
            Assert.Equal(mapping[ControllerElement.KeySemicolon], "semicolon");
            Assert.Equal(mapping[ControllerElement.KeyComma], "comma");
            Assert.Equal(mapping[ControllerElement.KeyPeriod], "period");
            Assert.Equal(mapping[ControllerElement.KeySlash], "slash");
            Assert.Equal(mapping[ControllerElement.KeyBracketLeft], "leftbracket");
            Assert.Equal(mapping[ControllerElement.KeyBracketRight], "rightbracket");
            Assert.Equal(mapping[ControllerElement.KeyBackslash], "backslash");
            Assert.Equal(mapping[ControllerElement.KeyRightAlt], "alt");
            Assert.Equal(mapping[ControllerElement.KeyRightCtrl], "ctrl");
            Assert.Equal(mapping[ControllerElement.KeyRightShift], "shift");
            Assert.Equal(mapping[ControllerElement.KeyNum0], "num0");
            Assert.Equal(mapping[ControllerElement.KeyNum1], "num1");
            Assert.Equal(mapping[ControllerElement.KeyNum2], "num2");
            Assert.Equal(mapping[ControllerElement.KeyNum3], "num3");
            Assert.Equal(mapping[ControllerElement.KeyNum4], "num4");
            Assert.Equal(mapping[ControllerElement.KeyNum5], "num5");
            Assert.Equal(mapping[ControllerElement.KeyNum6], "num6");
            Assert.Equal(mapping[ControllerElement.KeyNum7], "num7");
            Assert.Equal(mapping[ControllerElement.KeyNum8], "num8");
            Assert.Equal(mapping[ControllerElement.KeyNum9], "num9");
            Assert.Equal(mapping[ControllerElement.KeyNumPeriod], "kp_period");
            Assert.Equal(mapping[ControllerElement.KeyNumPlus], "add");
            Assert.Equal(mapping[ControllerElement.KeyNumMinus], "subtract");
            Assert.Equal(mapping[ControllerElement.KeyNumEnter], "enter");
            Assert.Equal(mapping[ControllerElement.KeyF1], "f1");
            Assert.Equal(mapping[ControllerElement.KeyF2], "f2");
            Assert.Equal(mapping[ControllerElement.KeyF3], "f3");
            Assert.Equal(mapping[ControllerElement.KeyF4], "f4");
            Assert.Equal(mapping[ControllerElement.KeyF5], "f5");
            Assert.Equal(mapping[ControllerElement.KeyF6], "f6");
            Assert.Equal(mapping[ControllerElement.KeyF7], "f7");
            Assert.Equal(mapping[ControllerElement.KeyF8], "f8");
            Assert.Equal(mapping[ControllerElement.KeyF9], "f9");
            Assert.Equal(mapping[ControllerElement.KeyF10], "f10");
            Assert.Equal(mapping[ControllerElement.KeyF11], "f11");
            Assert.Equal(mapping[ControllerElement.KeyF12], "f12");
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
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var template = new InputTemplate<IRetroArchInput>(mapcol, 0);
            string serializedValue = new InputSerializer(serializer).Serialize(template, mapping).Replace(Environment.NewLine, "");
            Assert.Equal(
                TestUtilities.GetStringResource("Configurations.ExampleInput.cfg").Replace(Environment.NewLine, ""),
                serializedValue);
        }
        [Fact]
        public void IniSerialize_Test()
        {
            var serializer = new IniConfigurationSerializer(new BooleanMapping("true", "false"), "nul");
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var template = new InputTemplate<IRetroArchInput>(mapcol, 0);
            string serializedValue = new InputSerializer(serializer).Serialize(template, mapping).Replace(Environment.NewLine, "");
            Assert.Equal(
                TestUtilities.GetStringResource("Configurations.ExampleInput.ini").Replace(Environment.NewLine, ""),
                serializedValue);
        }
    }
}
