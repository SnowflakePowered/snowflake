using System.Collections.Generic;

namespace Snowflake.Configuration
{
    public interface IConfigurationSection<out T> : IConfigurationSection where T: class, IConfigurationSection<T>
    {
        T Configuration { get; }
    }

    public interface IConfigurationSection
    {
        IList<IConfigurationOption> Options { get; }
        string Destination { get; }
        string Description { get; }
        string DisplayName { get; }
        string SectionName { get; }
        IDictionary<string, IConfigurationValue> Values { get; }
        object this[string key] { get; set; }
    }
}
