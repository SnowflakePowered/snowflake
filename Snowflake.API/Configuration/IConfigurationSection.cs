using System.Collections.Generic;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a serializable section in a configuration 
    /// </summary>
    /// <typeparam name="T">The type of configuration</typeparam>
    public interface IConfigurationSection<out T> : IConfigurationSection where T: class, IConfigurationSection<T>
    {
        T Configuration { get; }
    }

    public interface IConfigurationSection
    {
        IConfigurationSectionDescriptor Descriptor { get; }
        IDictionary<string, IConfigurationValue> Values { get; }
        object this[string key] { get; set; }
    }
}
