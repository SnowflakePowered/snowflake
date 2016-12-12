using Snowflake.Support.Remoting.Framework.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class RequestResponse
    {
        public object Response { get; }
        public RequestError Error { get; }
        internal RequestResponse(object response, RequestError error)
        {
            this.Response = response;
            this.Error = error;
        }
    }
}
