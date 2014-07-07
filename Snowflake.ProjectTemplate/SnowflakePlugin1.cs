using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.API.Base;

namespace $safeprojectname$
{
    public class SnowflakePlugin1 : BasePlugin
    {
        public SnowflakePlugin1():base(Assembly.GetExecutingAssembly())
        {
            this.InitConfiguration();
        }
    }
}
