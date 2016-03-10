using System;
using System.IO;
using Snowflake.Emulator.Input.Constants;
using Xunit;

namespace Snowflake.Controller.Tests
{
    public class GamepadAbstractionDabataseTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.GetTempFileName();
            IGamepadAbstractionDatabase database = new GamepadAbstractionDatabase(filename);
            Assert.NotEmpty(database.GetAllGamepadAbstractions());
        }

        [Fact]
        public void AddNewAbstraction_Test()
        {
            string filename = Path.GetTempFileName();
            IGamepadAbstractionDatabase database = new GamepadAbstractionDatabase(filename);
            IGamepadAbstraction abstraction = new GamepadAbstraction("~test", ControllerProfileType.NULL_PROFILE);
            database.SetGamepadAbstraction("~test", abstraction);
            Assert.NotNull(database.GetGamepadAbstraction("~test"));
        }

        [Theory]
        [InlineData("KeyboardDevice")]
        [InlineData("XInputDevice1")]
        [InlineData("XInputDevice2")]
        [InlineData("XInputDevice3")]
        [InlineData("XInputDevice4")]
        public void ChangeAbstraction_Test(string deviceName)
        {
            string filename = Path.GetTempFileName();
            IGamepadAbstractionDatabase database = new GamepadAbstractionDatabase(filename);
            IGamepadAbstraction abstraction = database.GetGamepadAbstraction(deviceName);
            abstraction.A = KeyboardConstants.KEY_Z;
            database.SetGamepadAbstraction(deviceName, abstraction);
            Assert.Equal(KeyboardConstants.KEY_Z, database.GetGamepadAbstraction(deviceName).A);
        }
        [Theory]
        [InlineData("KeyboardDevice")]
        [InlineData("XInputDevice1")]
        [InlineData("XInputDevice2")]
        [InlineData("XInputDevice3")]
        [InlineData("XInputDevice4")]
        public void RemoveAbstraction_Test(string deviceName)
        {
            string filename = Path.GetTempFileName();
            IGamepadAbstractionDatabase database = new GamepadAbstractionDatabase(filename);
            database.RemoveGamepadAbstraction(deviceName);
            Assert.Null(database.GetGamepadAbstraction(deviceName));
        }
    }
}
