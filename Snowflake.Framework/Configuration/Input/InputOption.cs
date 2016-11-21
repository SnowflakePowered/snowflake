using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {

        public InputOptionType InputOptionType { get; }
        public ControllerElement TargetElement { get; }
        public string OptionName { get; }
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
