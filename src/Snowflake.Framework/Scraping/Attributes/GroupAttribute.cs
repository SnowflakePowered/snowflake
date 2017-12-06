using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GroupAttribute : Attribute
    {
        public string GroupName { get; }
        public string GroupOn { get; }
        public GroupAttribute(string groupName, string groupOn)
        {
            this.GroupName = groupName;
            this.GroupOn = GroupOn;
        }
    }
}
