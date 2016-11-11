using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Configuration;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Configuration.Internal;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    [ConfigurationFile("#retroarch", "retroarch.cfg")]
    public interface RetroArchConfiguration : IConfigurationCollection<RetroArchConfiguration>
    {
        [SerializableSection("#retroarch")]
        BuiltinConfiguration BuiltinConfiguration { get; }

        [SerializableSection("#retroarch")]
        MenuConfiguration MenuConfiguration { get; }

        [SerializableSection("#retroarch")]
        GameConfiguration GameConfiguration { get; }

        [SerializableSection("#retroarch")]
        RecordConfiguration RecordConfiguration { get; }

        [SerializableSection("#retroarch")]
        PauseConfiguration PauseConfiguration { get; }

        [SerializableSection("#retroarch")]
        LocationConfiguration LocationConfiguration { get; }

        [SerializableSection("#retroarch")]
        NetworkConfiguration NetworkConfiguration { get; }

        [SerializableSection("#retroarch")]
        LogConfiguration LogConfiguration { get; }

        [SerializableSection("#retroarch")]
        InputConfiguration InputConfiguration { get; }

        [SerializableSection("#retroarch")]
        NetplayConfiguration NetplayConfiguration { get; }

        [SerializableSection("#retroarch")]
        XmbConfiguration XmbConfiguration { get; }

        [SerializableSection("#retroarch")]
        StdinConfiguration StdinConfiguration { get; }

        [SerializableSection("#retroarch")]
        CoreConfiguration CoreConfiguration { get; }

        [SerializableSection("#retroarch")]
        CameraConfiguration CameraConfiguration { get; }

        [SerializableSection("#retroarch")]
        BundleConfiguration BundleConfiguration { get; }

        [SerializableSection("#retroarch")]
        RewindConfiguration RewindConfiguration { get; }

        [SerializableSection("#retroarch")]
        SaveConfiguration SaveConfiguration { get; }

        [SerializableSection("#retroarch")]
        UserConfiguration UserConfiguration { get; }

        [SerializableSection("#retroarch")]
        ThreadedConfiguration ThreadedConfiguration { get; }

        [SerializableSection("#retroarch")]
        DirectoryConfiguration DirectoryConfiguration { get; }

        [SerializableSection("#retroarch")]
        UiConfiguration UiConfiguration { get; }

        [SerializableSection("#retroarch")]
        VideoConfiguration VideoConfiguration { get; }

        [SerializableSection("#retroarch")]
        AudioConfiguration AudioConfiguration { get; }

        [SerializableSection("#retroarch")]
        FramethrottleConfiguration FrametrottleConfiguration { get; }

        [SerializableSection("#retroarch")]
        ConfigConfiguration ConfigConfiguration { get; }

        /*RetroArchConfiguration() : base(new KeyValuePairConfigurationSerializer(BooleanMapping.LowercaseBooleanMapping, "nul", "="), "retroarch.cfg")
        {

        }*/
    }
}