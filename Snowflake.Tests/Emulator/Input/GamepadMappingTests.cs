using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tests;
using Newtonsoft.Json;
using Xunit;
namespace Snowflake.Emulator.Input.Tests
{
    public class GamepadMappingTests
    {
        [Fact]
        public void GamepadMapping_LoadTest()
        {
            string _mappings = TestUtilities.GetStringResource("Mapping.GamepadMapping.json");
            var mappingData = JsonConvert.DeserializeObject<IDictionary<string, string>>(_mappings);
            var gamepadMapping = new GamepadMapping(mappingData);
            Assert.NotNull(gamepadMapping);
        }

        [Theory]
        [InlineData("GAMEPAD_A")]
        [InlineData("GAMEPAD_B")]
        [InlineData("GAMEPAD_X")]
        [InlineData("GAMEPAD_Y")]
        [InlineData("GAMEPAD_START")]
        [InlineData("GAMEPAD_SELECT")]
        [InlineData("GAMEPAD_L1")]
        [InlineData("GAMEPAD_L2")]
        [InlineData("GAMEPAD_L3")]
        [InlineData("GAMEPAD_R1")]
        [InlineData("GAMEPAD_R2")]
        [InlineData("GAMEPAD_R3")]
        [InlineData("GAMEPAD_L_X_RIGHT")]
        [InlineData("GAMEPAD_L_X_LEFT")]
        [InlineData("GAMEPAD_L_Y_UP")]
        [InlineData("GAMEPAD_L_Y_DOWN")]
        [InlineData("GAMEPAD_R_X_RIGHT")]
        [InlineData("GAMEPAD_R_X_LEFT")]
        [InlineData("GAMEPAD_R_Y_DOWN")]
        [InlineData("GAMEPAD_R_Y_UP")]
        [InlineData("GAMEPAD_GUIDE")]
        [InlineData("GAMEPAD_DPAD_UP")]
        [InlineData("GAMEPAD_DPAD_DOWN")]
        [InlineData("GAMEPAD_DPAD_LEFT")]
        [InlineData("GAMEPAD_DPAD_RIGHT")]
        public void GamepadMapping_EqualityTests(string mappingKey)
        {
            string _mappings = TestUtilities.GetStringResource("Mapping.GamepadMapping.json");
            var mappingData = JsonConvert.DeserializeObject<IDictionary<string, string>>(_mappings);
            var gamepadMapping = new GamepadMapping(mappingData);
            Assert.Equal(gamepadMapping[mappingKey], typeof(GamepadMapping).GetProperty(mappingKey).GetValue(gamepadMapping));
        }

        [Theory]
        [InlineData("GAMEPAD_A")]
        [InlineData("GAMEPAD_B")]
        [InlineData("GAMEPAD_X")]
        [InlineData("GAMEPAD_Y")]
        [InlineData("GAMEPAD_START")]
        [InlineData("GAMEPAD_SELECT")]
        [InlineData("GAMEPAD_L1")]
        [InlineData("GAMEPAD_L2")]
        [InlineData("GAMEPAD_L3")]
        [InlineData("GAMEPAD_R1")]
        [InlineData("GAMEPAD_R2")]
        [InlineData("GAMEPAD_R3")]
        [InlineData("GAMEPAD_L_X_RIGHT")]
        [InlineData("GAMEPAD_L_X_LEFT")]
        [InlineData("GAMEPAD_L_Y_UP")]
        [InlineData("GAMEPAD_L_Y_DOWN")]
        [InlineData("GAMEPAD_R_X_RIGHT")]
        [InlineData("GAMEPAD_R_X_LEFT")]
        [InlineData("GAMEPAD_R_Y_DOWN")]
        [InlineData("GAMEPAD_R_Y_UP")]
        [InlineData("GAMEPAD_GUIDE")]
        [InlineData("GAMEPAD_DPAD_UP")]
        [InlineData("GAMEPAD_DPAD_DOWN")]
        [InlineData("GAMEPAD_DPAD_LEFT")]
        [InlineData("GAMEPAD_DPAD_RIGHT")]
        public void GamepadMapping_IndexerTests(string mappingKey)
        {
            string _mappings = TestUtilities.GetStringResource("Mapping.GamepadMapping.json");
            var mappingData = JsonConvert.DeserializeObject<IDictionary<string, string>>(_mappings);
            var gamepadMapping = new GamepadMapping(mappingData);
            Assert.Equal(gamepadMapping[mappingKey], gamepadMapping[mappingKey]);
        }
    }
}
