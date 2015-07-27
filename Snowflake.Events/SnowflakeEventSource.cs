using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Events.CoreEvents.ModifyEvent;
using Snowflake.Events.ServiceEvents;

namespace Snowflake.Events
{
    public class SnowflakeEventSource
    {
        public void RegisterSnowflakeEvents(ISnowflakeEventManager eventManager)
        {
            eventManager.RegisterEvent<GameAddEventArgs>(this.GameAdd);
            eventManager.RegisterEvent<GameDeleteEventArgs>(this.GameDelete);
            eventManager.RegisterEvent<GamePreAddEventArgs>(this.GamePreAdd);
            eventManager.RegisterEvent<GamePreDeleteEventArgs>(this.GamePreDelete);
            eventManager.RegisterEvent<GameInfoScrapedEventArgs>(this.GameInfoScraped);
            eventManager.RegisterEvent<GameResultsScrapedEventArgs>(this.GameResultsScraped);
            eventManager.RegisterEvent<GameStartEventArgs>(this.GameStart);
            eventManager.RegisterEvent<GameQuitEventArgs>(this.GameQuit);
            eventManager.RegisterEvent<GameProcessQuitEventArgs>(this.GameProcessQuit);
            eventManager.RegisterEvent<GameProcessStartEventArgs>(this.GameProcessStart);

            eventManager.RegisterEvent<AjaxRequestReceivedEventArgs>(this.AjaxRequestReceived);
            eventManager.RegisterEvent<AjaxResponseSendingEventArgs>(this.AjaxResponseSending);

            eventManager.RegisterEvent<CoreLoadedEventArgs>(this.CoreLoaded);
            eventManager.RegisterEvent<CoreShutdownEventArgs>(this.CoreShutdown);

            eventManager.RegisterEvent<ServerStartEventArgs>(this.ServerStart);
            eventManager.RegisterEvent<ServerStopEventArgs>(this.ServerStop);

            eventManager.RegisterEvent<ModifyControllerProfileEventArgs>(this.ControllerProfileModify);
            eventManager.RegisterEvent<ModifyConfigurationFlagEventArgs>(this.ConfigurationFlagModify);
            eventManager.RegisterEvent<ModifyGameInfoEventArgs>(this.GameInfoModify);
            eventManager.RegisterEvent<ModifyPlatformPreferenceEventArgs>(this.PlatformPreferenceModify);
            eventManager.RegisterEvent<ModifyPortInputDeviceEventArgs>(this.PortInputDeviceModify);

        }
        //todo Register these events
        public event EventHandler<GameAddEventArgs> GameAdd;
        public event EventHandler<GameDeleteEventArgs> GameDelete;
        public event EventHandler<GamePreAddEventArgs> GamePreAdd;
        public event EventHandler<GamePreDeleteEventArgs> GamePreDelete;
        public event EventHandler<GameInfoScrapedEventArgs> GameInfoScraped;
        public event EventHandler<GameResultsScrapedEventArgs> GameResultsScraped;
        public event EventHandler<GameStartEventArgs> GameStart;
        public event EventHandler<GameQuitEventArgs> GameQuit;
        public event EventHandler<GameProcessQuitEventArgs> GameProcessQuit;
        public event EventHandler<GameProcessStartEventArgs> GameProcessStart;

        public event EventHandler<AjaxRequestReceivedEventArgs> AjaxRequestReceived;
        public event EventHandler<AjaxResponseSendingEventArgs> AjaxResponseSending;

        public event EventHandler<CoreLoadedEventArgs> CoreLoaded;
        public event EventHandler<CoreShutdownEventArgs> CoreShutdown;
        public event EventHandler<ServerStartEventArgs> ServerStart;
        public event EventHandler<ServerStopEventArgs> ServerStop;

        public event EventHandler<ModifyControllerProfileEventArgs> ControllerProfileModify;
        public event EventHandler<ModifyConfigurationFlagEventArgs> ConfigurationFlagModify;
        public event EventHandler<ModifyGameInfoEventArgs> GameInfoModify;
        public event EventHandler<ModifyPlatformPreferenceEventArgs> PlatformPreferenceModify;
        public event EventHandler<ModifyPortInputDeviceEventArgs> PortInputDeviceModify;

    }
}
