using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Snowflake.Scraping
{
    public abstract class Culler : ICuller
    {
        public Culler()
        {
            var targetAttribute = this.GetType().GetCustomAttribute<TargetAttribute>();
            if (targetAttribute == null)
            {
                throw new InvalidOperationException("Culler must specify target");
            }

            this.TargetType = targetAttribute.TargetType;
        }

        public string TargetType { get; }
        public abstract IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim);
    }
}
