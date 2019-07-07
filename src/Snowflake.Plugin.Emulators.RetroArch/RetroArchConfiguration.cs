using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Plugin.Emulators.RetroArch.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Configuration.Internal;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    [ConfigurationTarget("#retroarch")]
    public interface RetroArchConfiguration : IConfigurationCollection<RetroArchConfiguration>
    {
        [ConfigurationTargetMember("#retroarch")] BuiltinConfiguration BuiltinConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] MenuConfiguration MenuConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] GameConfiguration GameConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] RecordConfiguration RecordConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] PauseConfiguration PauseConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] LocationConfiguration LocationConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] NetworkConfiguration NetworkConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] LogConfiguration LogConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] InputConfiguration InputConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] NetplayConfiguration NetplayConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] XmbConfiguration XmbConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] StdinConfiguration StdinConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] CoreConfiguration CoreConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] CameraConfiguration CameraConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] BundleConfiguration BundleConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] RewindConfiguration RewindConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] SaveConfiguration SaveConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] UserConfiguration UserConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] ThreadedConfiguration ThreadedConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] DirectoryConfiguration DirectoryConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] UiConfiguration UiConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] VideoConfiguration VideoConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] AudioConfiguration AudioConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] FramethrottleConfiguration FrametrottleConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] ConfigConfiguration ConfigConfiguration { get; }
    }
}
