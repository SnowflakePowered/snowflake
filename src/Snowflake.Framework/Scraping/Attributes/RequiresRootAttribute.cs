using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresRootAttribute : Attribute
    {
        public string Child { get; }
        public RequiresRootAttribute(string child)
        {
            this.Child = child;
        }
    }
}