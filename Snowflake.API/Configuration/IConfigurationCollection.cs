using System.Collections.Generic;

namespace Snowflake.Configuration
{
    public interface IConfigurationCollection<out T> : IConfigurationCollection where T : class, IConfigurationCollection<T>
    {
        T Configuration { get; }
    }

    /// <summary>
    /// Represents a configuration collection
    /// 
    /// The sections of this collection are enumerated in order of declaration
    /// //todo more notes about configuration
    /// </summary>
    public interface IConfigurationCollection : IEnumerable<IConfigurationSection> 
    {
        IDictionary<string, string> Outputs { get; }
        IConfigurationSection this[string sectionName] { get; }
    }
}
