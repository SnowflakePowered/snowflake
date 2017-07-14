using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Requests
{
    public class Response
    {
        public static Response NoEndpointFoundResponse { get; }
        static Response() {
            Response.NoEndpointFoundResponse = new Response(null);
        }
        public Response(object payload)
        {

        }
    }
}
