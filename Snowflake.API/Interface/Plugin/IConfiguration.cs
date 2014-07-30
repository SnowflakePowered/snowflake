using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Snowflake.API.Interface.Plugin
{
    public interface IConfiguration
    {
        string ConfigurationFileName { get; }
        IDictionary<string, dynamic> Configuration { get; }
        void LoadConfiguration();
        void SaveConfiguration();
    }
}
