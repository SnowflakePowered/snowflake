using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public interface IResponseStatus
    {
        string Message { get; }
        string Type { get; }
        int Code { get; }
    }
}
