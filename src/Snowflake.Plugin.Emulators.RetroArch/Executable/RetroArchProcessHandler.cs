using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioned;

namespace Snowflake.Plugin.Emulators.RetroArch.Executable
{
    [Plugin("exe-retroarch-win64")]
    public class RetroArchProcessHandler : ProvisionedPlugin
    {
        public RetroArchProcessHandler(IPluginProvision provision) :
            base(provision)
        {
        }

        internal RetroArchProcessInfo GetProcessInfo(string romFilePath) => 
            new RetroArchProcessInfo(this.Provision.ResourceDirectory.FullName, romFilePath);
    }
}
