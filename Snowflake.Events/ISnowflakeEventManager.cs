using System;
namespace Snowflake.Events
{
    public interface ISnowflakeEventManager
    {
        EventHandler<T> GetEvent<T>() where T : SnowflakeEventArgs;
        void RaiseEvent<T>(T eventArgs) where T : SnowflakeEventArgs;
        void RegisterEvent<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;
        void Subscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;
        void UnregisterEvent<T>() where T : SnowflakeEventArgs;
        void Unsubscribe<T>(EventHandler<T> eventHandler) where T : SnowflakeEventArgs;
    }
}
