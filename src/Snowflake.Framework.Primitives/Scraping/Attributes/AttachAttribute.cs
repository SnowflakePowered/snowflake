using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttachAttribute : Attribute
    {
        public AttachTarget AttachTarget { get; }
        public AttachAttribute(AttachTarget target)
        {
            this.AttachTarget = target;
        }
    }
}
