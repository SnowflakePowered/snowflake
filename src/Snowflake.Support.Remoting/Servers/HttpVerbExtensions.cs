using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Support.Remoting.Servers
{
    internal static class HttpVerbExtensions
    {
        internal static EndpointVerb ToCrud(this HttpVerbs @this)
        {
            switch (@this)
            {
                case HttpVerbs.Get:
                    return EndpointVerb.Read;
                case HttpVerbs.Post:
                    return EndpointVerb.Create;
                case HttpVerbs.Delete:
                    return EndpointVerb.Delete;
                case HttpVerbs.Put:
                    return EndpointVerb.Update;
                default:
                    return EndpointVerb.Read;
            }
        }
    }
}
