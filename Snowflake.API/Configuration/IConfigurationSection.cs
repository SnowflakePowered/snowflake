using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public interface IConfigurationSection
    {
        string SectionName { get; }
        string DisplayName { get; }
        string ConfigurationFileName { get; }
    }
}
