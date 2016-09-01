using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Hotkey;
using Snowflake.Input.Controller;
using Xunit;
namespace Snowflake.Tests.Configuration
{
    public class HotkeyTriggerTests
    {
        [Fact]
        public void TriggerEqualityTests()
        {
            Assert.Equal(new HotkeyTrigger(), new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement));
        }
    }
}
