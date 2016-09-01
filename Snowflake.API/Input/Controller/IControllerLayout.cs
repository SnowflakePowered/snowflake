using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents the layout of a controller, real or virtual
    /// </summary>
    public interface IControllerLayout
    {
        /// <summary>
        /// The identifier or name of the layout.
        /// </summary>
        string LayoutID { get; }
        
        /// <summary>
        /// The friendly name of this layout
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Whether or not this is a real device layout or a virtual one.
        /// </summary>
        bool IsRealDevice { get; }

        /// <summary>
        /// The platforms this controller supports.
        /// </summary>
        IEnumerable<string> Platforms { get; }

        /// <summary>
        /// The actual collection of layout elements
        /// </summary>
        IControllerElementCollection Layout { get; }
    }
}
