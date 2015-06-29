using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Controller
{
    /// <summary>
    /// Represents a standard gamepad
    /// 2 shoulder buttons
    /// 2 shoulder triggers
    /// 1 4-cardinal direction d-pad
    /// 2 clickable analog sticks 
    /// Start Button
    /// Select Button
    /// </summary>
    public interface IGamepadAbstraction
    {
        string DeviceName { get; }

        string L1 { get; set; }
        string L2 { get; set; }
        string L3 { get; set; }
        string R1 { get; set; }
        string R2 { get; set; }
        string R3 { get; set; }

        string DpadUp { get; set;}
        string DpadDown { get; set; }
        string DpadLeft { get; set; }
        string DpadRight { get; set; }

        string RightAnalogXLeft { get; set; }
        string RightAnalogXRight { get; set; }
        string RightAnalogYUp { get; set; }
        string RightAnalogYDown { get; set; }

        string LeftAnalogXLeft { get; set; }
        string LeftAnalogXRight { get; set; }
        string LeftAnalogYUp { get; set; }
        string LeftAnalogYDown { get; set; }

        string Select { get; set; }
        string Start { get; set; }
    }
}
