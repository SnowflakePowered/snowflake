using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping.Attributes.Conditions
{
    public sealed class SeedHasChildrenAttribute : SeedConditionAttribute
    {
        public string ChildType { get; }
        public SeedHasChildrenAttribute(string rootType, string childType)
            : base(rootType)
        {
            this.ChildType = childType;
        }

        public override bool Check(ISeed seed, ISeedRootContext context)
        {
            return context.GetChildren(seed).Any();
        }
    }
}
