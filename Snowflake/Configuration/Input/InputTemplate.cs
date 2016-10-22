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
    [Obsolete]
    public abstract class InputTemplate : IInputTemplate
    {
        public int PlayerIndex { get; set; }
        public string SectionName { get; }
        public IEnumerable<IInputOption> InputOptions => this.inputOptions;
        public IEnumerable<IConfigurationOption> ConfigurationOptions => this.configurationOptions;
        private readonly IList<IInputOption> inputOptions;
        private readonly IList<IConfigurationOption> configurationOptions;
            
        protected InputTemplate(string sectionName)
        {
            this.SectionName = sectionName;
            this.inputOptions = this.GetInputProperties();
            this.configurationOptions = this.GetConfigProperties();
        }

        private IList<IInputOption> GetInputProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                where propertyInfo.IsDefined(typeof (InputOptionAttribute), true)
                select new InputOption(propertyInfo, this) as IInputOption).ToList();
        }

        private IList<IConfigurationOption> GetConfigProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                    where propertyInfo.IsDefined(typeof(ConfigurationOptionAttribute), true)
                    select new ConfigurationOption(propertyInfo, this) as IConfigurationOption).ToList();
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
                    .Where(option => option.TargetElement == element.LayoutElement)
                    .FirstOrDefault(
                        option => (option.InputOptionType == (InputOptionType.Controller | InputOptionType.Keyboard))
                                  || (option.InputOptionType == InputOptionType.Keyboard && deviceIsKeyboard)
                                  || (option.InputOptionType == InputOptionType.ControllerAxes && elementIsAxis)
                                  || (option.InputOptionType == InputOptionType.Controller && !deviceIsKeyboard));
                if (inputOption != null) inputOption.Value = element;
                //good lord.
            }
        }

    }
}
