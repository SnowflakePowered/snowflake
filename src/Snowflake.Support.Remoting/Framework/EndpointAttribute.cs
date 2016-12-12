using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointAttribute : Attribute
    {
        public RemotingVerbs Verb { get; }
        public string Path { get; }
        public EndpointAttribute(RemotingVerbs verb, string path)
        {
            this.Verb = verb;
            this.Path = path;
        }
    }
}
