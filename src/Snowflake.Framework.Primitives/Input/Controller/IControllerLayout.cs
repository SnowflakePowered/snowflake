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
        /// Gets the identifier or name of the layout.
        /// </summary>
        string LayoutID { get; }

        /// <summary>
        /// Gets the friendly name of this layout
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this is a real device layout or a virtual one.
        /// </summary>
        bool IsRealDevice { get; }

        /// <summary>
        /// Gets the platforms this controller supports.
        /// </summary>
        IEnumerable<string> Platforms { get; }

        /// <summary>
        /// Gets the actual collection of layout elements
        /// </summary>
        IControllerElementCollection Layout { get; }
    }
}
