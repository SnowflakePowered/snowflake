using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Framework.Tests.Configuration
{
    [ConfigurationSection("myconfig", "myconfig")]
    public partial interface MyConfiguration
    {
        [ConfigurationOption("Hello", true)]
        bool MyBoolean { get; set; }

        [ConfigurationOption("ss")]
        Guid MyResource { get; set; }



    }

    public partial interface MyConfiguration
    {
        [ConfigurationOption("myenum", MyEnum.World)]
        MyEnum MyEnum { get; set; }
    }


    public enum MyEnum
    {
        [SelectionOption("world")]
        Hello,
        [SelectionOption("world")]
        World
    }
}
