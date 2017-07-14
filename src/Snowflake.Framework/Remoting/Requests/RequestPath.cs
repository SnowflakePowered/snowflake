using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public class RequestPath : IRequestPath
    {
        public IImmutableList<string> PathNodes => this.path;
        private readonly IImmutableList<string> path;
        public RequestPath(params string[] path)
        {
            this.path = ImmutableList.Create(path);
        }
    }
}
