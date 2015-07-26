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
            }
        }
        /// <summary>
        /// Registers an event with the event manager
        /// </summary>
        /// <typeparam name="T">The SnowflakeEventArgs event arguments</typeparam>
        /// <param name="eventHandler">The event handler to register with. Can be null.</param>
        public void RegisterEvent<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (!eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = eventHandler;
            }
        }
        /// <summary>
        /// Removes an event from the event manager.
        /// Once removed, events of the type removed will no longer fire.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnregisterEvent<T>() where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                eventContainer[typeof(T)] = null;
                Delegate value;
                eventContainer.TryRemove(typeof(T), out value);
            }
        }
        /// <summary>
        /// Raise an event
        /// </summary>
        /// <typeparam name="T">The SnowflakeEventArgs event to raise</typeparam>
        /// <param name="eventArgs">The event arguments to raise the event with</param>
        public void RaiseEvent<T>(T eventArgs) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                var snowflakeEvent = GetEvent<T>();
                if (snowflakeEvent != null)
                {
                    snowflakeEvent(this, eventArgs);
                }
            }
        }
        /// <summary>
        /// Gets the event for the SnowflakeEventArgs.
        /// </summary>
        /// <remarks>Do not subscribe to the event.</remarks>
        /// <see cref="SnowflakeEventManager.Subscribe"/>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public EventHandler<T> GetEvent<T>() where T : SnowflakeEventArgs
        {
            Delegate eventHandler;
            eventContainer.TryGetValue(typeof(T), out eventHandler);
            return eventHandler as EventHandler<T>;

        }
        /// <summary>
        /// Subscribe to an event. 
        /// </summary>
        /// <typeparam name="T">The event to subscribe to</typeparam>
        /// <param name="eventHandler">The event handler</param>
        public void Subscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                if (eventContainer[typeof(T)] != null)
                {
                    eventContainer[typeof(T)] = (eventContainer[typeof(T)] as EventHandler<T>) + eventHandler;
                }
                else
                {
                    eventContainer[typeof(T)] = eventHandler;
                }
            }
        }
        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <typeparam name="T">The event to unsubscribe from</typeparam>
        /// <param name="eventHandler">The event handler to remove</param>
        public void Unsubscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs
        {
            if (eventContainer.ContainsKey(typeof(T)))
            {
                if (eventContainer[typeof(T)] != null)
                {
                    eventContainer[typeof(T)] = (eventContainer[typeof(T)] as EventHandler<T>) - eventHandler;
                }
            }
        }
    }
}
