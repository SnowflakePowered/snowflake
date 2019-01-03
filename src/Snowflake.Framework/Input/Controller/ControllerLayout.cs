using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.JsonConverters;
using Snowflake.Model.Game;

namespace Snowflake.Input.Controller
{
    // should support everything including numeric keypads. buttons are for generic use, such as shaking actions
    [JsonConverter(typeof(ControllerLayoutConverter))]
    internal class ControllerLayout : IControllerLayout
    {
        /// <inheritdoc/>
        public ControllerId LayoutId { get; }

        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public bool IsRealDevice { get; }

        /// <inheritdoc/>
        public IEnumerable<PlatformId> Platforms { get; }

        /// <inheritdoc/>
        public IControllerElementCollection Layout { get; }

        internal ControllerLayout(ControllerId layoutId, IEnumerable<PlatformId> platforms, string friendlyName,
            IControllerElementCollection layout, bool isRealDevice = false)
        {
            this.LayoutId = layoutId;
            this.FriendlyName = friendlyName;
            this.IsRealDevice = isRealDevice;
            this.Platforms = platforms;
            this.Layout = layout;
        }
    }
}
