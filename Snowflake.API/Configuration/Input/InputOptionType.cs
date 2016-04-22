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
    public enum InputOptionType
    {
        /// <summary>
        /// This input option accepts any input type
        /// </summary>
        Any,
        /// <summary>
        /// This input option accepts only keyboard keys
        /// </summary>
        KeyboardKey,
        /// <summary>
        /// This input option accepts only controller element mappings.
        /// </summary>
        ControllerElement,
        /// <summary>
        /// This input option is restricted to axes only
        /// </summary>
        ControllerElementAxes
    }
}
