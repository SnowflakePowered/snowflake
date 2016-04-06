using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    public interface IControllerLayout
    {
        /// <summary>
        /// The identifier or name of the layout.
        /// </summary>
        string LayoutName { get; }
        string FriendlyName { get; }
        bool IsRealDevice { get; }
        IEnumerable<string> PlatformsWhitelist { get; }
        IControllerElementCollection Layout { get; }
    }
}
