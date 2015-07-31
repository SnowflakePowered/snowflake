using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Configuration
{
    public interface IPluginSelectOption
    {
        string Key { get; }
        string Description { get; }
    }
}
