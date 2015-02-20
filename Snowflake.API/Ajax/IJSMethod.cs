using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Snowflake.Ajax
{
    public interface IJSMethod
    {
        MethodInfo MethodInfo { get; }
        Func<IJSRequest, IJSResponse> Method { get; }
    }
}
