using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration
{
   public abstract class InputSerialzier
   {


       protected abstract string SerializeElement(MappedControllerElement element);

       protected virtual string SerializeKeyboard(MappedControllerElement element)
       {
           return nameof(element.DeviceKeyboardKey).Remove(0, 3); //Key is 3 characters
       }
    

    }
}
