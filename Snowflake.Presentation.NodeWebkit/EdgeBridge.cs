using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Presentation.NodeWebkit
{
    public class EdgeBridge
    {
        public async Task<object> HelloWorld(object input)
        {
            return await Task.FromResult<string>(string.Format("Hello {0} From Snowflake", input));
        }

       
    }
}
