using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static ISnowflakeEventManager EventSource;

        /// <summary>
        /// Stores the EventHandlers
        /// </summary>
        private ConcurrentDictionary<Type, Delegate> eventContainer = new ConcurrentDictionary<Type, Delegate>();

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
            if (!eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = eventHandler;
            }
        }
        
        public void UnregisterEvent<T>() where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = null;
                Delegate value;
                eventContainer.TryRemove(typeof(T), out value);
            }
        }
     
        public void RaiseEvent<T>(T eventArgs) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                var snowflakeEvent = GetEvent<T>();
                snowflakeEvent?.Invoke(this, eventArgs);
            }
        }
      
        public EventHandler<T> GetEvent<T>() where T : SnowflakeEventArgs
        {
            Delegate eventHandler;
            eventContainer.TryGetValue(typeof(T), out eventHandler);
            return eventHandler as EventHandler<T>;

        }
   
        public void Subscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = (eventContainer?[typeof(T)] as EventHandler<T>) + eventHandler ?? eventHandler;
            }
        }
     
        public void Unsubscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = (eventContainer?[typeof(T)] as EventHandler<T>) - eventHandler ?? null;
            }
        }
    }
}
