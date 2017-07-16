using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ResourceAttribute : Attribute
    {
        public string[] Path { get; }
        public ResourceAttribute(params string[] resourcePath)
        {
            this.Path = resourcePath;
        }
    }
}
