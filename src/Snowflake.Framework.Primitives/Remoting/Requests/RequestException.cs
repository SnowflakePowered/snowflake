using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public class RequestException : Exception
    {
        public int ErrorCode { get; }
        public RequestException(string message, int error) : base (message)
        {
            this.ErrorCode = error;
        }
    }
}
