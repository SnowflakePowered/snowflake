using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public interface IInputOption
    {
        InputOptionType InputOptionType { get; }
        ControllerElement TargetElement { get; }
        string OptionName { get; }
    }
}