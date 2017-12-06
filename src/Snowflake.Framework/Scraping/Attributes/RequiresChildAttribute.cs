using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresChildAttribute : Attribute
    {
        public string Child { get; }
        public RequiresChildAttribute(string child)
        {
            this.Child = child;
        }
    }
}
