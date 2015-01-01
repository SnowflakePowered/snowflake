using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Snowflake.Plugin
{
    public interface IPluginConfiguration
    {
        string ConfigurationFileName { get; }
        IDictionary<string, dynamic> Configuration { get; }
        void LoadConfiguration();
        void SaveConfiguration();
    }
}
