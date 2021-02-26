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
        string A { get; }
    }

    interface IB
        : IA
    {
        string B { get; }
    }
}
