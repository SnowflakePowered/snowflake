using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Events.CoreEvents.ModifyEvent;
namespace Snowflake.Events
{
    public partial class SnowflakeEventSource
    {
        public event EventHandler<ModifyControllerProfileEventArgs> ControllerProfileModify;
        public event EventHandler<ModifyGameFlagEventArgs> GameFlagModify;
        public event EventHandler<ModifyGameInfoEventArgs> GameInfoModify;
        public event EventHandler<ModifyPlatformPreferenceEventArgs> PlatformPreferenceModify;
        public event EventHandler<ModifyPortInputDeviceEventArgs> PortInputDeviceModify;

        public void OnControllerProfileModify(ModifyControllerProfileEventArgs e)
        {
            if (this.ControllerProfileModify != null)
            {
                this.ControllerProfileModify(this, e);
            }
        }
        public void OnGameFlagModify(ModifyGameFlagEventArgs e)
        {
            if (this.GameFlagModify != null)
            {
                this.GameFlagModify(this, e);
            }
        }
        public void OnGameInfoModify(ModifyGameInfoEventArgs e)
        {
            if (this.GameInfoModify != null)
            {
                this.GameInfoModify(this, e);
            }
        }
        public void OnPlatformPreferenceModify(ModifyPlatformPreferenceEventArgs e)
        {
            if (this.PlatformPreferenceModify != null)
            {
                this.PlatformPreferenceModify(this, e);
            }
        }
        public void OnPortInputDeviceModify(ModifyPortInputDeviceEventArgs e)
        {
            if (this.PortInputDeviceModify != null)
            {
                this.PortInputDeviceModify(this, e);
            }
        }

    }
}
