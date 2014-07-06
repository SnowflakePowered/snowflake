using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Snowflake.API.Interface
{
    public interface IConfiguration
    {
        string ConfigurationFileName { get; }
        Dictionary<string, dynamic> Configuration { get; }
        void LoadConfiguration();
        void SaveConfiguration();
    }
}
