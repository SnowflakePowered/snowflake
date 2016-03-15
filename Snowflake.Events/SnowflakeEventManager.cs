using System;
using System.Collections.Concurrent;

namespace Snowflake.Events
{
    /// <summary>
    /// A Dependency Injection-inspired Event Manager for Snowflake.
    /// </summary>
    public class SnowflakeEventManager : ISnowflakeEventManager
    {
        SnowflakeEventManager()
        {

        }
        /// <summary>
        /// Singleton Events source from which all events propagate
        /// </summary>
        public static ISnowflakeEventManager EventSource { get; set; }

        /// <summary>
        /// Stores the EventHandlers
        /// </summary>
        private readonly ConcurrentDictionary<Type, Delegate> eventContainer = new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Stores the event handlers
        /// </summary>
        private readonly ConcurrentDictionary<string, MulticastDelegate> eventHandlers =
            new ConcurrentDictionary<string, MulticastDelegate>();
        /// <summary>
        /// Initiates the EventSource singleton.
        /// </summary>
        public static void InitEventSource()
        {
            if (SnowflakeEventManager.EventSource == null)
            {
                SnowflakeEventManager.EventSource = new SnowflakeEventManager();
                var stdEvents = new StandardEvents();
                stdEvents.RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            }
        }

        public bool Contains<T>() where T : SnowflakeEventArgs
        {
            return this.eventContainer.ContainsKey(typeof(T));
        }

        public void RegisterEvent<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (!this.eventContainer.ContainsKey(typeof(T)))
            {
                this.eventContainer[typeof(T)] = eventHandler;
            }
        }
        
        public void UnregisterEvent<T>() where T : SnowflakeEventArgs
        {
            if (this.eventContainer.ContainsKey(typeof(T)))
            {
                this.eventContainer[typeof(T)] = null;
                Delegate value;
                this.eventContainer.TryRemove(typeof(T), out value);
            }
        }
     
        public void RaiseEvent<T>(T eventArgs) where T : SnowflakeEventArgs
        {
            if (this.eventContainer.ContainsKey(typeof(T)))
            {
                var snowflakeEvent = this.GetEvent<T>();
                snowflakeEvent?.Invoke(this, eventArgs);
            }
        }
      
        public EventHandler<T> GetEvent<T>() where T : SnowflakeEventArgs
        {
            Delegate eventHandler;
            this.eventContainer.TryGetValue(typeof(T), out eventHandler);
            return eventHandler as EventHandler<T>;

        }
   
        public void Subscribe<T>(string eventHandlerKey, EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (this.eventContainer.ContainsKey(typeof(T)))
            {
                this.eventHandlers[eventHandlerKey] = eventHandler;
                this.eventContainer[typeof(T)] = (this.eventContainer?[typeof(T)] as EventHandler<T>) + (this.eventHandlers[eventHandlerKey] as EventHandler<T>) ?? this.eventHandlers[eventHandlerKey];
            }
        }
     
        public void Unsubscribe<T>(string eventHandlerKey) where T : SnowflakeEventArgs
        {
            if (this.eventContainer.ContainsKey(typeof(T)))
            {
                EventHandler<T> eventHandler = this.eventHandlers[eventHandlerKey] as EventHandler<T>;
                this.eventContainer[typeof (T)] = Delegate.Remove(
                    (this.eventContainer?[typeof (T)] as EventHandler<T>), eventHandler);
            }
        }
    }
}
