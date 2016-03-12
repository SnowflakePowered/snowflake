namespace Snowflake.Configuration
{
    public interface IConfigurationSerializer
    {
        IConfigurationTypeMapper TypeMapper { get; set; }
        string SerializeValue(object value);
        string SerializeLine<T>(string key, T value);
        string Serialize(IConfigurationSection configurationSection);
    }
}