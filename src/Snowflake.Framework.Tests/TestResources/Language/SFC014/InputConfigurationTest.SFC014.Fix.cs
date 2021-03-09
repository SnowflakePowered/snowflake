namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;
    using Snowflake.Configuration.Input;
    using Snowflake.Input.Controller;
    using Snowflake.Input.Device;

    [InputConfiguration("TestSection")]
    public partial interface TestInterface
    {
        [InputOption("TestProperty", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        DeviceCapability TestProperty { set; get; }
    }
}
