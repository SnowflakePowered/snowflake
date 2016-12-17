using Snowflake.Support.Remoting.Framework.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class RequestException : Exception
    {
        public int ErrorCode { get; }
        public RequestException(string message, int error) : base (message)
        {
            this.ErrorCode = error;
        }

        public RequestError ToError()
        {
            return new RequestError(this);
        }
    }
}
