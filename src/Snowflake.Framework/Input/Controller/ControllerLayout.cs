using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.JsonConverters;
using Snowflake.Platform;

namespace Snowflake.Input.Controller
{
    // should support everything including numeric keypads. buttons are for generic use, such as shaking actions
    [JsonConverter(typeof(ControllerLayoutConverter))]
    public class ControllerLayout : IControllerLayout
    {
        /// <inheritdoc/>
        public string LayoutID { get; }

        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public bool IsRealDevice { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Platforms { get; }

        /// <inheritdoc/>
        public IControllerElementCollection Layout { get; }

        public ControllerLayout(string layoutId, IEnumerable<string> platforms, string friendlyName,
            IControllerElementCollection layout, bool isRealDevice = false)
        {
            this.LayoutID = layoutId;
            this.FriendlyName = friendlyName;
            this.IsRealDevice = isRealDevice;
            this.Platforms = platforms;
            this.Layout = layout;
        }
    }
}
