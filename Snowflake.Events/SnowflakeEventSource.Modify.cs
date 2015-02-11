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
        public event EventHandler<ModifyConfigurationFlagEventArgs> ConfigurationFlagModify;
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
        public void OnConfigurationFlagModify(ModifyConfigurationFlagEventArgs e)
        {
            if (this.ConfigurationFlagModify != null)
            {
                this.ConfigurationFlagModify(this, e);
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
