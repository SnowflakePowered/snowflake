using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Snowflake.Ajax
{
    internal sealed class JSMethod : IJSMethod
    {
        public MethodInfo MethodInfo { get; private set; }
        public Func<IJSRequest, IJSResponse> Method { get; private set; }
        public JSMethod(MethodInfo methodInfo, Func<IJSRequest, IJSResponse> method)
        {
            this.Method = method;
            this.MethodInfo = methodInfo;
        }
    }
}
