using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Thrown when attempting to create an invalid controller ID.
    /// </summary>
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
