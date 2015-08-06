namespace Snowflake.Controller
{
    public class ControllerInput : IControllerInput
    {
        public bool IsAnalog { get; }
        public string InputName { get; }
        public string GamepadAbstraction { get; }

        public ControllerInput(string inputName, string gamepadAbstraction, bool isAnalog = false)
        {
            this.InputName = inputName;
            this.IsAnalog = isAnalog;
            this.GamepadAbstraction = gamepadAbstraction;
        }
    }
}
