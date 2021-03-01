using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    [ConfigurationCollection]
    public partial interface IA
    {
        AType A { get; }
        AType B { get; }
    }

    [ConfigurationSection("(CHANGE ME!) sectionName", "(CHANGE ME!) displayName")]
    public partial interface AType
    {
        [ConfigurationOption("ashdhj", "no", DisplayName = "no")]
        string Non { get; set; }
    }


    [InputConfiguration("input")]
    public partial interface InputTest
    {
        [ConfigurationOption("ashdhj", "no", DisplayName = "no")]
        string Non { get; set; }

        
    }
    //[ConfigurationCollection]
    //partial interface IB
    //    : IA
    //{
    //    string B { get; }
    //    string A { get; }
    //}

}
