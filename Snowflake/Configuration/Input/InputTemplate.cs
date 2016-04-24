using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    public abstract class InputTemplate : IInputTemplate
    {
        public int PlayerIndex { get; set; }
        public IEnumerable<IInputOption> InputOptions => this.inputOptions;
        private readonly IList<IInputOption> inputOptions;
        protected InputTemplate()
        {
            this.inputOptions = this.GetInputProperties();
        }

        private IList<IInputOption> GetInputProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                where propertyInfo.IsDefined(typeof (InputOptionAttribute), true)
                select new InputOption(propertyInfo, this) as IInputOption).ToList();
        }

        public virtual void SetInputValues(IMappedControllerElementCollection mappedElements, IInputDevice inputDevice,
            int playerIndex)
        {
            this.PlayerIndex = playerIndex;
            bool deviceIsKeyboard = inputDevice.DeviceLayout.Layout.Keyboard != null;

            foreach (IMappedControllerElement element in mappedElements)
            {
                //if the device is not a keyboard or no element and the device type is either axes type
                bool elementIsAxis = (!deviceIsKeyboard && element.DeviceElement != ControllerElement.NoElement)
                     && ((inputDevice.DeviceLayout.Layout[element.DeviceElement].Type == ControllerElementType.AxisNegative)
                        || (inputDevice.DeviceLayout.Layout[element.DeviceElement].Type == ControllerElementType.AxisPositive));

                //Find the correct input option for this target.
                var inputOption = this.InputOptions
                    .Where(option => option.TargetElement == element.TargetElement)
                    .FirstOrDefault(
                        option => (option.InputOptionType == InputOptionType.Any)
                                  || (option.InputOptionType == InputOptionType.KeyboardKey && deviceIsKeyboard)
                                  || (option.InputOptionType == InputOptionType.ControllerElementAxes && elementIsAxis)
                                  || (option.InputOptionType == InputOptionType.ControllerElement && !deviceIsKeyboard));
                if (inputOption != null) inputOption.Value = element;
                //good lord.
            }
        }

    }
}
