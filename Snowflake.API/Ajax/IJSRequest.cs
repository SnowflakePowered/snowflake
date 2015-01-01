using System;
using System.Collections.Generic;
namespace Snowflake.Ajax
{
    public interface IJSRequest
    {
        string GetParameter(string paramKey);
        string MethodName { get; }
        IDictionary<string, string> MethodParameters { get; }
        string NameSpace { get; }
    }
}
