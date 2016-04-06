using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Emulator.Input;
using Snowflake.JsonConverters;
using Snowflake.Platform;

namespace Snowflake.Input.Controller
{
    //should support everything including numeric keypads. buttons are for generic use, such as shaking actions

    [JsonConverter(typeof(ControllerLayoutConverter))]
    public class ControllerLayout : IControllerLayout
    {
        public string LayoutName { get; }
        public string FriendlyName { get; }
        public bool IsRealDevice { get; }
        public IEnumerable<string> PlatformsWhitelist { get; }
        public IControllerElementCollection Layout { get; }

        public ControllerLayout(string layoutName, IEnumerable<string> platformsWhitelist, string friendlyName,
            IControllerElementCollection layout, bool isRealDevice = false)
        {
            this.LayoutName = layoutName;
            this.FriendlyName = friendlyName;
            this.IsRealDevice = isRealDevice;
            this.PlatformsWhitelist = platformsWhitelist;
            this.Layout = layout;
        }

    }
}
