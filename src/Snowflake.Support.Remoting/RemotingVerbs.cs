using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Support.Remoting
{
    public enum RequestVerb
    {
        /// <summary>
        ///  Equivalent to HTTP POST
        /// </summary>
        Create,
        /// <summary>
        /// Equivalent to HTTP GET
        /// </summary>
        Read,
        /// <summary>
        /// Equivalent to HTTP PUT
        /// </summary>
        Update,
        /// <summary>
        /// Equivalent to HTTP DELETE
        /// </summary>
        Delete
    }

    internal static class RequestVerbExtensions
    {
        internal static RequestVerb ToCrud(this HttpVerbs @this)
        {
            switch(@this)
            {
                case HttpVerbs.Get:
                    return RequestVerb.Read;
                case HttpVerbs.Post:
                    return RequestVerb.Create;
                case HttpVerbs.Delete:
                    return RequestVerb.Delete;
                case HttpVerbs.Put:
                    return RequestVerb.Update;
                default:
                    return RequestVerb.Read;
            }
        }
    }
}
