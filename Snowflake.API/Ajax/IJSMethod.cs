using System;
using System.Reflection;

namespace Snowflake.Ajax
{
    public interface IJSMethod
    {
        MethodInfo MethodInfo { get; }
        Func<IJSRequest, IJSResponse> Method { get; }
    }
}
