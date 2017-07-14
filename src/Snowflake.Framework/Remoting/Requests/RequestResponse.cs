
using Snowflake.Remoting.Requests;

namespace Snowflake.Remoting.Requests
{
    public class RequestResponse : IRequestResponse
    {
        public static RequestResponse NoEndpointFoundResponse { get; }
        static RequestResponse() {
            RequestResponse.NoEndpointFoundResponse = new RequestResponse(null);
        }
        public object Response { get; }

        public RequestResponse(object payload)
        {
            this.Response = payload;
        }
    }
}
