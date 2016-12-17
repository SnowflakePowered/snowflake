using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Errors
{
    public class RequestError
    {
        public string Message { get; }
        public string Type { get; }
        public int Code { get; }

        internal RequestError(string message, int code)
        {
            this.Message = message;
            this.Type = this.GetType().Name;
            this.Code = code;
        }

        public RequestError(RequestException e)
        {
            this.Message = e.Message;
            this.Type = e.GetType().Name.Replace("Exception", "Error");
            this.Code = e.ErrorCode;
        }

        public RequestError(Exception e, int code = 503)
        {
            this.Message = $"Unhandled exception occurred: {e.InnerException?.Message ?? e.Message}";
            this.Type = e.InnerException?.GetType().Name ?? e.GetType().Name;
            this.Code = code;
        }
    }
}
