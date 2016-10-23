using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.DynamicConfiguration.Input
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
