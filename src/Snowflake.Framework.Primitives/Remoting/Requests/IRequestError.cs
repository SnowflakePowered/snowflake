using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public interface IRequestError
    {
        string Message { get; }
        string Type { get; }
        int Code { get; }
    }
}
