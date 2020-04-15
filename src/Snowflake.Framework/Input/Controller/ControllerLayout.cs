using System;
using System.Collections;
using System.Collections.Generic;
using Snowflake.Model.Game;

namespace Snowflake.Input.Controller
{
    // should support everything including numeric keypads. buttons are for generic use, such as shaking actions
    internal class ControllerLayout : IControllerLayout
    {
        /// <inheritdoc/>
        public ControllerId ControllerID { get; }

        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public IEnumerable<PlatformId> Platforms { get; }

        /// <inheritdoc/>
        public IControllerElementCollection Layout { get; }

        internal ControllerLayout(ControllerId layoutId, IEnumerable<PlatformId> platforms, string friendlyName,
            IControllerElementCollection layout)
        {
            this.ControllerID = layoutId;
            this.FriendlyName = friendlyName;
            this.Platforms = platforms;
            this.Layout = layout;
        }
    }
}
