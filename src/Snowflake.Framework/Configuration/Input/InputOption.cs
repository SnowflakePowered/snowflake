using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {
        /// <inheritdoc/>
        public InputOptionType OptionType { get; }

        /// <inheritdoc/>
        public ControllerElement TargetElement { get; }

        /// <inheritdoc/>
        public string OptionName { get; }

        /// <inheritdoc/>
        public string KeyName { get; }

        internal InputOption(InputOptionAttribute attribute, string keyName)
        {
            this.OptionName = attribute.OptionName;
            this.OptionType = attribute.InputOptionType;
            this.TargetElement = attribute.TargetElement;
            this.KeyName = keyName;
        }
    }
}
