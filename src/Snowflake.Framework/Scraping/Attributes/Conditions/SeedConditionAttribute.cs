using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes.Conditions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class SeedConditionAttribute : Attribute
    {
        public string TargetType { get; }

        protected SeedConditionAttribute(string targetType)
        {
            this.TargetType = targetType;
        }

        public abstract bool Check(ISeed seed, ISeedRootContext context);
    }
}
