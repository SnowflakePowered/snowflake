using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class NamedArgumentAttribute : Attribute
    {
        public string ShortName { get; }
        public string LongName { get; }
        public string Description { get; }
        
        public NamedArgumentAttribute(string shortName, string longName, string description)
        {
            this.ShortName = shortName;
            this.LongName = longName;
            this.Description = description;
        }
    }
}
