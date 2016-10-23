using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.DynamicConfiguration.Input
{
    public interface IInputOption
    {
        InputOptionType InputOptionType { get; }
        ControllerElement TargetElement { get; }
        string OptionName { get; }
    }
}