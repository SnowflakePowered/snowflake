using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ParentAttribute : Attribute
    {
        public string ParentType { get; }
        public ParentAttribute(string parentType)
        {
            this.ParentType = parentType;
        }
    }
}
