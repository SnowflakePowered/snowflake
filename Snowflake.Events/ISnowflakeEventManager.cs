using System;
namespace Snowflake.Events
{
    public interface ISnowflakeEventManager
    {
        /// <summary>
        /// Gets the event for the SnowflakeEventArgs.
        /// </summary>
        /// <remarks>Do not subscribe to the event.</remarks>
        /// <see cref="SnowflakeEventManager.Subscribe"/>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        EventHandler<T> GetEvent<T>() where T : SnowflakeEventArgs;
        /// <summary>
        /// Raise an event
        /// </summary>
        /// <typeparam name="T">The SnowflakeEventArgs event to raise</typeparam>
        /// <param name="eventArgs">The event arguments to raise the event with</param>
        void RaiseEvent<T>(T eventArgs) where T : SnowflakeEventArgs;
        /// <summary>
        /// Registers an event with the event manager
        /// </summary>
        /// <typeparam name="T">The SnowflakeEventArgs event arguments</typeparam>
        /// <param name="eventHandler">The event handler to register with. Can be null.</param>
        void RegisterEvent<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;
        /// <summary>
        /// Subscribe to an event. 
        /// </summary>
        /// <typeparam name="T">The event to subscribe to</typeparam>
        /// <param name="eventHandler">The event handler</param>
        void Subscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;
        /// <summary>
        /// Removes an event from the event manager.
        /// Once removed, events of the type removed will no longer fire.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnregisterEvent<T>() where T : SnowflakeEventArgs;
        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <typeparam name="T">The event to unsubscribe from</typeparam>
        /// <param name="eventHandler">The event handler to remove</param>
        void Unsubscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;

        /// <summary>
        /// Check if the event manager has a certain event registered
        /// </summary>
        /// <typeparam name="T">The event to check</typeparam>
        bool Contains<T>() where T : SnowflakeEventArgs;
    }
}
