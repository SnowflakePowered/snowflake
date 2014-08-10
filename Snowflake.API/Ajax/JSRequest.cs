using System.Collections.Generic;

namespace Snowflake.Ajax
{
    public class JSRequest
    {
        public string MethodName { get; private set; }
        public string NameSpace { get; private set; }
        public IDictionary<string, string> MethodParameters { get; private set; }
        public JSRequest(string nameSpace, string methodName, IDictionary<string, string> parameters)
        {
            this.NameSpace = nameSpace;
            this.MethodName = methodName;
            this.MethodParameters = parameters;
        }
    }
}
