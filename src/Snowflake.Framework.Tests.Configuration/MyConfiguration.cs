using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    [ConfigurationSection("myconfig", "myconfig")]
    partial interface MyConfiguration
    {
        [ConfigurationOption("myOption", false)]
        bool MyBoolean { get; set; }
    }
}
