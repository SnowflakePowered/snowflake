using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public interface IRequestResponse
    {
        object Response { get; }
        IResponseStatus Status { get; }
    }
}
