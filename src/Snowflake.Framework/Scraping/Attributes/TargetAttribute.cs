using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TargetAttribute : Attribute
    {
        public string TargetType { get; }
        public TargetAttribute(string targetType)
        {
            this.TargetType = targetType;
        }
    }
}
