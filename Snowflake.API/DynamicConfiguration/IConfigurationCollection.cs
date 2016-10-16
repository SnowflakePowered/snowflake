using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.DynamicConfiguration
{
    public interface IConfigurationCollection<out T> : IConfigurationCollection where T : class, IConfigurationCollection<T>
    {
        T Configuration { get; }
    }

    public interface IConfigurationCollection : IEnumerable<IConfigurationSection> 
    {
        IDictionary<string, IConfigurationSection> Sections { get; }
        IDictionary<string, string> Outputs { get; }
        IConfigurationSection this[string sectionName] { get; }

    }
}
