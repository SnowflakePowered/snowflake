using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {

        public InputOptionType InputOptionType { get; }
        public ControllerElement TargetElement { get; }
        public string OptionName { get; }


        internal InputOption(InputOptionAttribute attribute)
        {
            this.OptionName = attribute.OptionName;
            this.InputOptionType = attribute.InputOptionType;
            this.TargetElement = attribute.TargetElement;
        }
    }
}
