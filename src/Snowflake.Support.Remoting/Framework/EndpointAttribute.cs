using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting
{
    public class EndpointAttribute : Attribute
    {
        public string Path { get; }
        public string[] Params { get; }
        public EndpointAttribute(string path)
        {
            this.Path = path;
            this.Params = path.Split('/');
        }
    }
}
