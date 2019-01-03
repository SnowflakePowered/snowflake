using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Controller
{
    public class InvalidControllerIdException : Exception
    {
        internal InvalidControllerIdException(string controllerId)
            : base($"{controllerId} is not a valid Stone Controller ID. " +
                  $"ControllerIDs must have an underscore and end in either" +
                  $" CONTROLLER, DEVICE, or LAYOUT")
        {
        }
    }
}
