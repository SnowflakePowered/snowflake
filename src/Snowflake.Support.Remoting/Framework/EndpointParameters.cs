using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    internal class EndpointParameters
    {
        public IDictionary<string, string> Url { get; }
        public JObject Body { get; }

        public EndpointParameters(IDictionary<string, string> getParams, dynamic postParams)
        {
            this.Url = getParams;
            this.Body = postParams;
        }
    }
}
