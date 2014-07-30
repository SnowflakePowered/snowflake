using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Core.API.JSAPI
{
    public class JSRequest
    {
        public string MethodName { get; private set; }
        public IDictionary<string, string> MethodParameters { get; private set; }
        public JSRequest(string methodName, IDictionary<string, string> parameters)
        {
            this.MethodName = methodName;
            this.MethodParameters = parameters;
        }
    }
}
