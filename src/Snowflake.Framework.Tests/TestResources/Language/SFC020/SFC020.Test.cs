namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;
    using Snowflake.Configuration.Input;
    using Snowflake.Input.Controller;
    using Snowflake.Input.Device;

    [InputConfiguration("Test")]
    public partial interface TestInterface
    {
        [ConfigurationOption("TestProperty", DeviceCapability.None)]
        [InputOption("TestProperty", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        DeviceCapability TestProperty { get; }
    }
}
