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

            eventManager.RegisterEvent(this.AjaxRequestReceived);
            eventManager.RegisterEvent(this.AjaxResponseSending);

            eventManager.RegisterEvent(this.CoreLoaded);
            eventManager.RegisterEvent(this.CoreShutdown);
            eventManager.RegisterEvent(this.PluginLoaded);

            eventManager.RegisterEvent(this.ServerStart);
            eventManager.RegisterEvent(this.ServerStop);

            eventManager.RegisterEvent(this.GameInfoModify);


        }
        public event EventHandler<GameAddEventArgs> GameAdd;
        public event EventHandler<GameDeleteEventArgs> GameDelete;
        public event EventHandler<GamePreAddEventArgs> GamePreAdd;
        public event EventHandler<GamePreDeleteEventArgs> GamePreDelete;



        public event EventHandler<AjaxRequestReceivedEventArgs> AjaxRequestReceived;
        public event EventHandler<AjaxResponseSendingEventArgs> AjaxResponseSending;

        public event EventHandler<CoreLoadedEventArgs> CoreLoaded;
        public event EventHandler<CoreShutdownEventArgs> CoreShutdown;
        public event EventHandler<ServerStartEventArgs> ServerStart;
        public event EventHandler<ServerStopEventArgs> ServerStop;
        public event EventHandler<PluginLoadedEventArgs> PluginLoaded;

        public event EventHandler<ModifyGameInfoEventArgs> GameInfoModify;

    }
}
