using System.Collections.Generic;

namespace Snowflake.Configuration
{
    public interface IConfigurationCollection<out T> : IConfigurationCollection where T : class, IConfigurationCollection<T>
    {
        T Configuration { get; }
    }

    public interface IConfigurationCollection : IEnumerable<KeyValuePair<string, IConfigurationSection>>
    {
        IConfigurationCollectionDescriptor Descriptor { get; }
        IConfigurationSection this[string sectionName] { get; }

    }
}
