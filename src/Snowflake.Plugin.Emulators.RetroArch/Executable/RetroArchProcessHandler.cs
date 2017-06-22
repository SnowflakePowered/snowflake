using Snowflake.Extensibility;


namespace Snowflake.Plugin.Emulators.RetroArch.Executable
{
    [Plugin("exe-retroarch-win64")]
    public class RetroArchProcessHandler : Extensibility.Plugin
    {
        public RetroArchProcessHandler(string appDataDirectory, ILogger logger, IPluginProperties pluginProperties) : 
            base(appDataDirectory, logger, pluginProperties)
        {
        }

        public RetroArchProcessHandler(string appDataDirectory) : base(appDataDirectory)
        {
        }

        internal RetroArchProcessInfo GetProcessInfo(string romFilePath) => new RetroArchProcessInfo(this.PluginDataPath, romFilePath);
    }
}
