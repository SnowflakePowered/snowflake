using System;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Events.CoreEvents.ModifyEvent;
using Snowflake.Events.ServiceEvents;

namespace Snowflake.Events
{
    public class StandardEvents
    {
        public void RegisterSnowflakeEvents(ISnowflakeEventManager eventManager)
        {
            eventManager.RegisterEvent(this.GameAdd);
            eventManager.RegisterEvent(this.GameDelete);
            eventManager.RegisterEvent(this.GamePreAdd);
            eventManager.RegisterEvent(this.GamePreDelete);
            eventManager.RegisterEvent(this.GameInfoScraped);
            eventManager.RegisterEvent(this.GameResultsScraped);
        /*    eventManager.RegisterEvent(this.GameStart);
            eventManager.RegisterEvent(this.GameQuit);
            eventManager.RegisterEvent(this.GameProcessQuit);
            eventManager.RegisterEvent(this.GameProcessStart);*/

            eventManager.RegisterEvent(this.AjaxRequestReceived);
            eventManager.RegisterEvent(this.AjaxResponseSending);

            eventManager.RegisterEvent(this.CoreLoaded);
            eventManager.RegisterEvent(this.CoreShutdown);
            eventManager.RegisterEvent(this.PluginLoaded);

            eventManager.RegisterEvent(this.ServerStart);
            eventManager.RegisterEvent(this.ServerStop);

      /*      eventManager.RegisterEvent(this.ControllerProfileModify);
            eventManager.RegisterEvent(this.ConfigurationFlagModify);
            eventManager.RegisterEvent(this.GameInfoModify);
            eventManager.RegisterEvent(this.PlatformPreferenceModify);
            eventManager.RegisterEvent(this.PortInputDeviceModify);*/

        }
        public event EventHandler<GameAddEventArgs> GameAdd;
        public event EventHandler<GameDeleteEventArgs> GameDelete;
        public event EventHandler<GamePreAddEventArgs> GamePreAdd;
        public event EventHandler<GamePreDeleteEventArgs> GamePreDelete;
        public event EventHandler<GameInfoScrapedEventArgs> GameInfoScraped;
        public event EventHandler<GameResultsScrapedEventArgs> GameResultsScraped;
        /*public event EventHandler<GameStartEventArgs> GameStart;
        public event EventHandler<GameQuitEventArgs> GameQuit;
        public event EventHandler<GameProcessQuitEventArgs> GameProcessQuit;
        public event EventHandler<GameProcessStartEventArgs> GameProcessStart;*/

        public event EventHandler<AjaxRequestReceivedEventArgs> AjaxRequestReceived;
        public event EventHandler<AjaxResponseSendingEventArgs> AjaxResponseSending;

        public event EventHandler<CoreLoadedEventArgs> CoreLoaded;
        public event EventHandler<CoreShutdownEventArgs> CoreShutdown;
        public event EventHandler<ServerStartEventArgs> ServerStart;
        public event EventHandler<ServerStopEventArgs> ServerStop;
        public event EventHandler<PluginLoadedEventArgs> PluginLoaded;

     /*   public event EventHandler<ModifyControllerProfileEventArgs> ControllerProfileModify;
        public event EventHandler<ModifyConfigurationFlagEventArgs> ConfigurationFlagModify;
        public event EventHandler<ModifyGameInfoEventArgs> GameInfoModify;
        public event EventHandler<ModifyPlatformPreferenceEventArgs> PlatformPreferenceModify;
        public event EventHandler<ModifyPortInputDeviceEventArgs> PortInputDeviceModify;*/

    }
}
