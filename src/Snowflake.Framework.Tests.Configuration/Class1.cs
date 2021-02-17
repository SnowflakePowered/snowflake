using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    class Class1
        : MyConfiguration
    {
        bool MyConfiguration.MyBoolean { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void X()
        {
            (this as MyConfiguration).MyBoolean = true;
        }
    }
}
