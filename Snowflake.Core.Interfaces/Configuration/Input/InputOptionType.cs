using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// The type of input the option accepts
    /// </summary>
    [Flags]
    public enum InputOptionType
    {

        /// <summary>
        /// This input option accepts only controller element mappings.
        /// </summary>
        Controller = 1 << 0,
        /// <summary>
        /// This input option is restricted to axes only
        /// </summary>
        ControllerAxes = 1 << 1,
        /// <summary>
        /// This input option accepts only keyboard keys
        /// </summary>
        Keyboard = 1 << 2
    }
}
