using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration.Input
{
    public class InputOption : IInputOption
    {

        public InputOptionType InputOptionType { get; }
        public ControllerElement TargetElement { get; }

        [JsonIgnore]
        public IMappedControllerElement Value
        {
            get { return this.propertyInfo.GetValue(this.instance) as IMappedControllerElement; }
            set { this.propertyInfo.SetValue(this.instance, value); }
        }


        private readonly PropertyInfo propertyInfo;
        private readonly IInputTemplate instance;

        internal InputOption(PropertyInfo propertyInfo, IInputTemplate instance)
        {
            this.propertyInfo = propertyInfo;
            this.instance = instance;
            var attribute = propertyInfo.GetCustomAttribute<InputOptionAttribute>();
            this.InputOptionType = attribute.InputOptionType;
            this.TargetElement = attribute.TargetElement;
        }
    }
}
