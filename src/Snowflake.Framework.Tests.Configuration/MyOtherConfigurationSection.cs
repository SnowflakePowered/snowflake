using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    [ConfigurationSection("myconfig", "myconfig")]
    public partial interface MyOtherConfiguration
    {
        [ConfigurationOption("Hello", true)]
        bool MyBoolean { get; set; }

        [ConfigurationOption("myenum", MyEnum.World)]
        MyEnum MyOtherEnum { get; set; }

        [ConfigurationOption("ss")]
        Guid MyResource { get; set; }

    }
}
