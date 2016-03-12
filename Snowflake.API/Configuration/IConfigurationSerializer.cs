namespace Snowflake.Configuration
{
    public interface IConfigurationSerializer
    {
        IConfigurationTypeMapper TypeMapper { get; set; }
        string SerializeValue(object value);
        string SerializeLine<T>(string key, T value);
        string SerializeIterableLine<T>(string key, T value, int iteration);
        string Serialize(IConfigurationSection configurationSection);
        string SerializeIterable(IConfigurationSection iterableConfigurationSection, int iteration);
    }
}