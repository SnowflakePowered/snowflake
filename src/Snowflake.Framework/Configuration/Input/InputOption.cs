using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {
        /// <inheritdoc/>
        public InputOptionDeviceType DeviceType { get; }

        /// <inheritdoc/>
        public ControllerElement TargetElement { get; }

        /// <inheritdoc/>
        public string OptionName { get; }

        /// <inheritdoc/>
        public string KeyName { get; }

        internal InputOption(InputOptionAttribute attribute, string keyName)
        {
            this.OptionName = attribute.OptionName;
            this.DeviceType = attribute.InputOptionType;
            this.TargetElement = attribute.TargetElement;
            this.KeyName = keyName;
        }
    }
}
