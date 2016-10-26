using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationFile("#example", "example")]
    public interface ExampleConfigurationCollection: IConfigurationCollection<ExampleConfigurationCollection>
    {
        [ConfigurationSection("example", "Example", "#example")]
       ExampleConfigurationSection ExampleConfiguration { get; set; }
      
    }
}
