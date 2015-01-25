using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform
{
    public interface IPlatformControllerPorts
    {
        /// <summary>
        /// The controller in first port of the platform
        /// </summary>
        string Port1 { get; set; }
        /// <summary>
        /// The controller in second port of the platform
        /// </summary>
        string Port2 { get; set; }
        /// <summary>
        /// The controller in third port of the platform
        /// </summary>
        string Port3 { get; set; }
        /// <summary>
        /// The controller in fourth port of the platform
        /// </summary>
        string Port4 { get; set; }
        /// <summary>
        /// The controller in fifth port of the platform
        /// </summary>
        string Port5 { get; set; }
        /// <summary>
        /// The controller in sixth port of the platform
        /// </summary>
        string Port6 { get; set; }
        /// <summary>
        /// The controller in seventh port of the platform
        /// </summary>
        string Port7 { get; set; }
        /// <summary>
        /// The controller in eigth port of the platform
        /// </summary>
        string Port8 { get; set; }


    }
}
