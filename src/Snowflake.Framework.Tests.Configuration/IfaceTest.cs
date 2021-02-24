using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    interface IA
    {
        string A { get; }
    }

    interface IB
        : IA
    {
        string B { get; }
    }

    class BImpl
        : IB
    {
        string IB.B { get; }

        string IA.A => throw new NotImplementedException();
        //string IB.A { get; } // Can't do this.
    }
}
