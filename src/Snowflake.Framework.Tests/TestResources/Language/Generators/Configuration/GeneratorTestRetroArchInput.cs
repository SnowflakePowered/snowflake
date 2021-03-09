using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Language.Generators.Test
{
    [InputConfiguration("input")]
    public partial interface IRetroArchInput
    {
        [ConfigurationOption("input_device_p{N}", 0)]
        int InputDevice { get; set; }

        [InputOption("input_player{N}_turbo", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurbo { get; }
    }
}
