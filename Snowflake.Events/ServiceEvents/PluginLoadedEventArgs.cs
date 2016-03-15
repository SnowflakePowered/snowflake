using Snowflake.Extensibility;
using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class PluginLoadedEventArgs : SnowflakeEventArgs
    {
        public IPlugin LoadedPlugin { get; }
        public PluginLoadedEventArgs(ICoreService eventCoreInstance, IPlugin loadedPlugin) : base(eventCoreInstance)
        {
            this.LoadedPlugin = loadedPlugin;
        }
    }
}
