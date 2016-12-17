using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointAttribute : Attribute
    {
        public RequestVerb Verb { get; }
        public string Path { get; }
        public EndpointAttribute(RequestVerb verb, string path)
        {
            this.Verb = verb;
            this.Path = path;
        }
    }
}
