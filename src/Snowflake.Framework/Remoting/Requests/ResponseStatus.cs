using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public sealed class ResponseStatus : IResponseStatus
    {
        public string Message { get; }
        public string Type { get; }
        public int Code { get; }

        private const string NotFoundError = "ErrorNotFound";
        private const string MissingParameterError = "ErrorMissingParameter";


        public static IResponseStatus OkStatus(EndpointVerb verb, IRequestPath requestPath)
        {
            return new ResponseStatus($"{verb}::{String.Join(":", requestPath.PathNodes)} succeeded.", 200);
        }

        public static IResponseStatus NotFoundStatus(EndpointVerb verb, IRequestPath requestPath)
        {
            return new ResponseStatus($"Could not find endpoint {verb}::{String.Join(":", requestPath.PathNodes)}.", 404, 
                ResponseStatus.NotFoundError);
        }

        public static IResponseStatus MissingParameterStatus(EndpointVerb verb, IRequestPath requestPath)
        {
            return new ResponseStatus($"Missing a parameter for the call {verb}::{String.Join(":", requestPath.PathNodes)}.", 400,
                ResponseStatus.MissingParameterError);
        }

        public static IResponseStatus UnhandledErrorStatus(EndpointVerb verb, IRequestPath requestPath, Exception e, int code = 503)
        {
            return new ResponseStatus($"Unhandled exception occurred during {verb}::{String.Join(":", requestPath.PathNodes)} : {e.InnerException?.Message ?? e.Message}", 
                code, $"Error{e.GetType().Name}");
        }

        public ResponseStatus(string message, int code, string errorType = "OK")
        {
            this.Message = message;
            this.Type = errorType;
            this.Code = code;
        }
    }
}
