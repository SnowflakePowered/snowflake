using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {
        /// <inheritdoc/>
        public InputOptionType InputOptionType { get; }

        /// <inheritdoc/>
        public ControllerElement TargetElement { get; }

        /// <inheritdoc/>
        public string OptionName { get; }

        /// <inheritdoc/>
        public string KeyName { get; }

        internal InputOption(InputOptionAttribute attribute, string keyName)
        {
            this.OptionName = attribute.OptionName;
            this.InputOptionType = attribute.InputOptionType;
            this.TargetElement = attribute.TargetElement;
            this.KeyName = keyName;
        }
    }
}
