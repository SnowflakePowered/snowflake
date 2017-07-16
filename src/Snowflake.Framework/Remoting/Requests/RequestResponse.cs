using Snowflake.Remoting.Requests;

namespace Snowflake.Remoting.Requests
{
    public sealed class RequestResponse : IRequestResponse
    {
        public object Response { get; }
        public IResponseStatus Status { get; }

        public RequestResponse(object payload, IResponseStatus status)
        {
            this.Response = payload;
            this.Status = status;
        }
    }
}
